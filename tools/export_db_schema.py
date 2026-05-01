import json
import re
from pathlib import Path


ROOT = Path(r"F:\Backend")
SNAPSHOT = ROOT / "VietausWebAPI.Infrastructure" / "Migrations" / "ApplicationDbContextModelSnapshot.cs"
CONFIG_ROOT = ROOT / "VietausWebAPI.Infrastructure" / "DatabaseContext" / "Configurations"
ENTITY_ROOT = ROOT / "VietausWebAPI.Core"
OUTPUT = ROOT / "outputs" / "db_schema_metadata.json"


ENTITY_START_RE = re.compile(r'modelBuilder\.Entity\("(?P<clr>[^"]+)", b =>')
PROPERTY_START_RE = re.compile(r'b\.Property<(?P<clr_type>[^>]+)>\("(?P<prop>[^"]+)"\)')
TOTABLE_RE = re.compile(r'b\.ToTable\("(?P<table>[^"]+)",\s*(?P<schema>\(string\)null|"[^"]+")\);')
HASKEY_RE = re.compile(r'b\.HasKey\((?P<keys>.*?)\)')
HASINDEX_RE = re.compile(r'b\.HasIndex\((?P<keys>.*?)\)')
HASFK_RE = re.compile(r'b\.HasOne\([^)]*\)\s*\.With(?:One|Many)\([^)]*\)\s*\.HasForeignKey\((?P<keys>.*?)\)')
STRING_RE = re.compile(r'"([^"]+)"')
HASCOLUMNNAME_RE = re.compile(r'\.HasColumnName\("([^"]+)"\)')
HASCOLUMNTYPE_RE = re.compile(r'\.HasColumnType\("([^"]+)"\)')
HASMAXLEN_RE = re.compile(r'\.HasMaxLength\((\d+)\)')
HASPRECISION_RE = re.compile(r'\.HasPrecision\((\d+),\s*(\d+)\)')
HASDEFAULTSQL_RE = re.compile(r'\.HasDefaultValueSql\("([^"]+)"\)')
HASDEFAULT_RE = re.compile(r'\.HasDefaultValue\(([^)]+)\)')
HASDBNAME_RE = re.compile(r'\.HasDatabaseName\("([^"]+)"\)')
HASNAME_RE = re.compile(r'\.HasName\("([^"]+)"\)')
HASCMT_RE = re.compile(r'\.HasComment\("([^"]+)"\)')


def split_blocks(text: str):
    lines = text.splitlines()
    blocks = []
    i = 0
    while i < len(lines):
        line = lines[i]
        if 'modelBuilder.Entity("' not in line:
            i += 1
            continue
        m = ENTITY_START_RE.search(line)
        if not m:
            i += 1
            continue
        start = i
        depth = 0
        i += 1
        while i < len(lines):
            depth += lines[i].count("{") - lines[i].count("}")
            if depth <= 0 and lines[i].strip() == "});":
                i += 1
                break
            i += 1
        block = "\n".join(lines[start:i])
        blocks.append((m.group("clr"), block))
    return blocks


def find_file_by_stem(root: Path, stem: str):
    matches = list(root.rglob(f"{stem}.cs"))
    if not matches:
        return None
    matches.sort(key=lambda p: (0 if "Entities" in p.parts else 1, len(str(p))))
    return matches[0]


def normalize_schema(schema_token: str):
    if schema_token == "(string)null":
        return "public"
    return schema_token.strip('"')


def summarize_purpose(entity_name: str, table_name: str):
    label = re.sub(r"([a-z0-9])([A-Z])", r"\1 \2", entity_name).replace("_", " ")
    if table_name.lower().startswith("aspnet"):
        return "Bang he thong Identity/phan quyen cua ASP.NET."
    return f"Luu du lieu cho thuc the {label}."


def summarize_column(column_name: str, is_pk: bool, is_fk: bool, default_value: str | None):
    if is_pk:
        return f"Khoa chinh cua ban ghi {column_name}."
    if is_fk:
        return f"Khoa ngoai/tham chieu qua cot {column_name}."
    name = column_name.lower()
    if name in {"createdat", "createddate", "create_date"}:
        return "Thoi diem tao ban ghi."
    if name in {"updatedat", "updateddate", "update_date"}:
        return "Thoi diem cap nhat ban ghi."
    if name.startswith("is") or name.startswith("has"):
        return f"Co danh dau trang thai/co dieu kien cua {column_name}."
    if default_value:
        return f"Cot {column_name}, co gia tri mac dinh {default_value}."
    words = re.sub(r"([a-z0-9])([A-Z])", r"\1 \2", column_name).replace("_", " ")
    return f"Thong tin {words.lower()}."


def parse_entity_block(clr_name: str, block: str):
    entity_name = clr_name.split(".")[-1]
    file_path = find_file_by_stem(ENTITY_ROOT, entity_name)
    table_match = TOTABLE_RE.search(block)
    if not table_match:
        return None

    table_name = table_match.group("table")
    schema_name = normalize_schema(table_match.group("schema"))
    property_records = []
    pk_columns = []
    unique_columns = set()
    indexes = []
    fk_columns = set()
    table_comment = None

    lines = block.splitlines()
    i = 0
    while i < len(lines):
        raw = lines[i].strip()
        if raw.startswith("b.Property<"):
            chain = [raw]
            i += 1
            while i < len(lines):
                chain.append(lines[i].strip())
                if lines[i].strip().endswith(";"):
                    break
                i += 1
            text = " ".join(chain)
            start = PROPERTY_START_RE.search(text)
            if not start:
                i += 1
                continue
            prop_name = start.group("prop")
            clr_type = start.group("clr_type")
            column_name = prop_name
            m = HASCOLUMNNAME_RE.search(text)
            if m:
                column_name = m.group(1)

            col_type = None
            m = HASCOLUMNTYPE_RE.search(text)
            if m:
                col_type = m.group(1)

            max_len = None
            m = HASMAXLEN_RE.search(text)
            if m:
                max_len = int(m.group(1))
                if not col_type:
                    col_type = f"varchar({max_len})"

            m = HASPRECISION_RE.search(text)
            if m and not col_type:
                col_type = f"numeric({m.group(1)},{m.group(2)})"

            default_value = None
            m = HASDEFAULTSQL_RE.search(text)
            if m:
                default_value = m.group(1)
            else:
                m = HASDEFAULT_RE.search(text)
                if m:
                    default_value = m.group(1).strip()

            required = ".IsRequired()" in text
            nullable = "Khong" if required else ("Co" if "?" in clr_type or "string" in clr_type else "Khong")
            if "Nullable" in clr_type:
                nullable = "Co"

            property_records.append(
                {
                    "property_name": prop_name,
                    "column_name": column_name,
                    "clr_type": clr_type,
                    "db_type": col_type or clr_type,
                    "nullable": nullable,
                    "default": default_value,
                    "comment": (HASCMT_RE.search(text).group(1) if HASCMT_RE.search(text) else None),
                    "value_generated": "OnAdd" if ".ValueGeneratedOnAdd()" in text else None,
                }
            )
        elif raw.startswith("b.HasKey("):
            chain = [raw]
            i += 1
            while i < len(lines):
                chain.append(lines[i].strip())
                if lines[i].strip().endswith(";"):
                    break
                i += 1
            text = " ".join(chain)
            pk_columns = STRING_RE.findall(HASKEY_RE.search(text).group("keys"))
        elif raw.startswith("b.HasIndex("):
            chain = [raw]
            i += 1
            while i < len(lines):
                chain.append(lines[i].strip())
                if lines[i].strip().endswith(";"):
                    break
                i += 1
            text = " ".join(chain)
            idx_cols_match = HASINDEX_RE.search(text)
            idx_cols = STRING_RE.findall(idx_cols_match.group("keys")) if idx_cols_match else []
            is_unique = ".IsUnique()" in text
            if is_unique:
                unique_columns.update(idx_cols)
            indexes.append(
                {
                    "columns": idx_cols,
                    "name": HASDBNAME_RE.search(text).group(1) if HASDBNAME_RE.search(text) else None,
                    "unique": is_unique,
                }
            )
        elif raw.startswith("b.HasOne("):
            chain = [raw]
            i += 1
            while i < len(lines):
                chain.append(lines[i].strip())
                if lines[i].strip().endswith(";"):
                    break
                i += 1
            text = " ".join(chain)
            m = HASFK_RE.search(text)
            if m:
                fk_columns.update(STRING_RE.findall(m.group("keys")))
        else:
            m = HASCMT_RE.search(raw)
            if m:
                table_comment = m.group(1)
        i += 1

    columns = []
    for prop in property_records:
        constraints = []
        if prop["property_name"] in pk_columns:
            constraints.append("PK")
        if prop["property_name"] in unique_columns:
            constraints.append("UNIQUE")
        if prop["property_name"] in fk_columns:
            constraints.append("FK")
        if prop["value_generated"]:
            constraints.append(f"Generated:{prop['value_generated']}")

        columns.append(
            {
                "column_name": prop["column_name"],
                "property_name": prop["property_name"],
                "data_type": prop["db_type"],
                "constraints": ", ".join(constraints),
                "nullable": prop["nullable"],
                "default": prop["default"] or "",
                "description": prop["comment"] or summarize_column(
                    prop["column_name"],
                    prop["property_name"] in pk_columns,
                    prop["property_name"] in fk_columns,
                    prop["default"],
                ),
            }
        )

    return {
        "entity_name": entity_name,
        "clr_name": clr_name,
        "schema": schema_name,
        "table_name": table_name,
        "full_table_name": f"{schema_name}.{table_name}",
        "purpose": table_comment or summarize_purpose(entity_name, table_name),
        "entity_file": str(file_path) if file_path else "",
        "configuration_file": str(find_file_by_stem(CONFIG_ROOT, f"{entity_name}Configuration")) if find_file_by_stem(CONFIG_ROOT, f"{entity_name}Configuration") else "",
        "primary_key": pk_columns,
        "indexes": indexes,
        "column_count": len(columns),
        "columns": columns,
    }


def main():
    text = SNAPSHOT.read_text(encoding="utf-8")
    tables = []
    for clr_name, block in split_blocks(text):
        parsed = parse_entity_block(clr_name, block)
        if parsed:
            tables.append(parsed)

    tables.sort(key=lambda x: (x["schema"].lower(), x["table_name"].lower()))
    data = {
        "source_snapshot": str(SNAPSHOT),
        "table_count": len(tables),
        "tables": tables,
    }
    OUTPUT.parent.mkdir(parents=True, exist_ok=True)
    OUTPUT.write_text(json.dumps(data, ensure_ascii=False, indent=2), encoding="utf-8")
    print(json.dumps({"output": str(OUTPUT), "table_count": len(tables)}, ensure_ascii=False))


if __name__ == "__main__":
    main()

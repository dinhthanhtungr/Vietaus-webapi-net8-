import fs from "node:fs/promises";
import path from "node:path";
import { fileURLToPath } from "node:url";
import { SpreadsheetFile, Workbook } from "@oai/artifact-tool";

const __dirname = path.dirname(fileURLToPath(import.meta.url));
const root = path.resolve(__dirname, "..");
const metadataPath = path.join(root, "outputs", "db_schema_metadata.json");
const outputDir = path.join(root, "outputs", "db_schema_export");
const outputPath = path.join(outputDir, "db-structure.xlsx");

const meta = JSON.parse(await fs.readFile(metadataPath, "utf8"));
const workbook = Workbook.create();

const summary = workbook.worksheets.add("Tong quan");
const detail = workbook.worksheets.add("Cau truc DB");
summary.showGridLines = false;
detail.showGridLines = false;

const headerFill = "#F3C623";
const sectionFill = "#FFF1A8";
const border = { color: "#7A6A00", style: "Continuous" };

summary.getRange("A1:F1").merge();
summary.getRange("A1").values = [["TONG QUAN CAU TRUC DATABASE"]];
summary.getRange("A1").format = {
  fill: "#B8860B",
  font: { bold: true, color: "#FFFFFF", size: 16 },
  horizontalAlignment: "center",
  verticalAlignment: "center",
};
summary.getRange("A2:F4").values = [
  ["Nguon snapshot", meta.source_snapshot, null, null, null, null],
  ["So bang", meta.table_count, null, null, null, null],
  ["Ngay xuat", new Date(), null, null, null, null],
];
summary.getRange("A2:A4").format = { fill: "#F6E7B0", font: { bold: true } };
summary.getRange("B4").setNumberFormat("yyyy-mm-dd hh:mm");

const summaryRows = [["Schema", "Bang", "So cot", "Khoa chinh", "Entity", "Config"]];
for (const table of meta.tables) {
  summaryRows.push([
    table.schema,
    table.table_name,
    table.column_count,
    table.primary_key.join(", "),
    table.entity_name,
    table.configuration_file ? path.basename(table.configuration_file) : "",
  ]);
}
summary.getRangeByIndexes(5, 0, summaryRows.length, 6).values = summaryRows;
summary.getRange("A6:F6").format = {
  fill: "#1F4E78",
  font: { bold: true, color: "#FFFFFF" },
  horizontalAlignment: "center",
};
summary.tables.add(`A6:F${5 + summaryRows.length}`, true, "DbSummary");
summary.freezePanes.freezeRows(6);
summary.getRange("A:F").format.autofitColumns();

let row = 1;
for (const table of meta.tables) {
  const endRow = row + table.columns.length + 1;
  detail.getRange(`A${row}:H${row}`).merge();
  detail.getRange(`A${row}`).values = [[`Bang: ${table.full_table_name}`]];
  detail.getRange(`A${row}`).format = {
    fill: headerFill,
    font: { bold: true, color: "#000000", size: 13 },
    horizontalAlignment: "left",
  };

  detail.getRange(`A${row + 1}:B${row + 1}`).values = [["Muc dich", table.purpose]];
  detail.getRange(`A${row + 1}`).format = { fill: sectionFill, font: { bold: true } };
  detail.getRange(`C${row + 1}:H${row + 1}`).values = [[
    "Entity",
    table.entity_name,
    "Primary Key",
    table.primary_key.join(", "),
    "Schema",
    table.schema,
  ]];
  detail.getRange(`C${row + 1}`).format = { fill: sectionFill, font: { bold: true } };
  detail.getRange(`E${row + 1}`).format = { fill: sectionFill, font: { bold: true } };
  detail.getRange(`G${row + 1}`).format = { fill: sectionFill, font: { bold: true } };

  const startDataRow = row + 2;
  detail.getRange(`A${startDataRow}:H${startDataRow}`).values = [[
    "Cot",
    "Property",
    "Kieu du lieu",
    "Rang buoc",
    "Cho phep null",
    "Mac dinh",
    "Mo ta",
    "Nguon",
  ]];
  detail.getRange(`A${startDataRow}:H${startDataRow}`).format = {
    fill: "#D9EAD3",
    font: { bold: true },
    horizontalAlignment: "center",
    wrapText: true,
  };

  const dataRows = table.columns.map((col) => [
    col.column_name,
    col.property_name,
    col.data_type,
    col.constraints,
    col.nullable,
    col.default,
    col.description,
    table.configuration_file ? path.basename(table.configuration_file) : "Snapshot",
  ]);
  detail.getRangeByIndexes(startDataRow, 0, dataRows.length, 8).values = dataRows;
  detail.getRange(`A${row}:H${endRow}`).format.borders = {
    top: border,
    bottom: border,
    left: border,
    right: border,
  };
  detail.getRange(`A${startDataRow + 1}:H${endRow}`).format.wrapText = true;
  row = endRow + 2;
}

detail.freezePanes.freezeRows(1);
detail.getRange("A:H").format.autofitColumns();
detail.getRange("A:H").format.wrapText = true;
detail.getRange("A:A").format.columnWidthPx = 170;
detail.getRange("B:B").format.columnWidthPx = 170;
detail.getRange("C:C").format.columnWidthPx = 140;
detail.getRange("D:D").format.columnWidthPx = 130;
detail.getRange("E:E").format.columnWidthPx = 95;
detail.getRange("F:F").format.columnWidthPx = 120;
detail.getRange("G:G").format.columnWidthPx = 360;
detail.getRange("H:H").format.columnWidthPx = 170;

await fs.mkdir(outputDir, { recursive: true });

const inspect = await workbook.inspect({
  kind: "table",
  range: "Tong quan!A1:F15",
  include: "values,formulas",
  tableMaxRows: 15,
  tableMaxCols: 6,
});
console.log(inspect.ndjson);

const render1 = await workbook.render({ sheetName: "Tong quan", range: "A1:F20", scale: 1.5, format: "png" });
await fs.writeFile(path.join(outputDir, "tong-quan.png"), new Uint8Array(await render1.arrayBuffer()));
const render2 = await workbook.render({ sheetName: "Cau truc DB", range: "A1:H30", scale: 1.5, format: "png" });
await fs.writeFile(path.join(outputDir, "cau-truc-db.png"), new Uint8Array(await render2.arrayBuffer()));

const xlsx = await SpreadsheetFile.exportXlsx(workbook);
await xlsx.save(outputPath);

console.log(JSON.stringify({ outputPath, tableCount: meta.table_count }));

using System;
using System.Collections.Generic;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.QAQCFeature.ProductInspectionFeature;

namespace VietausWebAPI.Core.Application.Features.Labs.Helpers
{
    public static class ExtractTestRowsHelper
    {
        public static List<PDFColumn> ExtractTestRows(PDFResultValue result, PDFSpecificationsValue? specs)
        {
            var list = new List<PDFColumn>();


            void Add(string label, string? value, string? specDefault, string? specOverride, string method, string unit)
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    list.Add(new PDFColumn
                    {
                        TestItem = label,
                        Specification = !string.IsNullOrWhiteSpace(specOverride) ? specOverride : specDefault,
                        Method = method,
                        Unit = unit,
                        Result = value
                    });
                }
            }


            Add("Appearance/Hình dạng", result.Shape, "Granule/Hạt hình trụ", specs?.Shape, "Vietaus std", "mm");
            Add("Pellet Size/Kích thước hạt", result.ParticleSize, "≤ 3.5", specs?.PelletSize, "Vietaus std", "mm");
            Add("Moisture/Độ ẩm", result.Moisture, "0 - 0.3", specs?.Moisture, "Vietaus std", "%");
            Add("Packing/Quy cách đóng gói", result.PackingSpec, "25", null, "Vietaus std", "Kg");
            Add("Storage temperature (Nhiệt độ bảo quản)", result.StorageCondition, "Room temperature \nNhiệt độ bảo quản", null, "Vietaus std", "°C");
            // Trường hợp dòng trống, gán "Yes" cứng luôn:
            Add("Storing condition (Điều kiện bảo quản)", "Yes", "Put on Pallet \nĐặt trên Pallet", null, "Vietaus std", "-");
            //Add("Shelf-life/Hạn sử dụng", result.DwellTime == true ? "YES" : null, "Còn > 1/2 thời gian sử dụng", specs?.DwellTime, "Vietaus std", "-");
            // Trường hợp dòng trống, gán "Yes" cứng luôn:
            Add("Shelf-life/Hạn sử dụng", "Yes", "Còn > 1/2 thời gian sử dụng", null, "Vietaus std", "-");
            Add("Colour Tolerance (Delta E)", result.ColorDeltaE, "< 1.0", specs?.DeltaE, "Vietaus std", "-");

            Add("MI/Chỉ số chảy", result.MFR, "-", specs?.MeltIndex, "ASTM D1238", "g/10min");
            Add("Dwell Time/Thời gian lưu máy", result.DwellTime == true ? "YES" : null, "-", specs?.DwellTime, "Vietaus std", "-");
            Add("Density/Tỷ trọng", result.Density, "-", specs?.Density, "ASTM D792", "g/cm³");
            Add("Tensile Strength/Độ bền kéo", result.TensileStrength, "-", specs?.TensileStrength, "Vietaus std", "MPa");
            Add("Elongation/Độ giãn dài", result.Elongation, "-", specs?.ElongationAtBreak, "Vietaus std", "%");
            Add("Flexural Strength/Độ bền uốn", result.FlexuralStrength, "-", specs?.FlexuralStrength, "Vietaus std", "MPa");
            Add("Flexural Modulus/Mô đun uốn", result.FlexuralModulus, "-", specs?.FlexuralModulus, "Vietaus std", "MPa");
            Add("Impact Resistance/Chịu va đập", result.ImpactResistance, "-", specs?.IzodImpactStrength, "Vietaus std", "kJ/m²");
            Add("Hardness/Độ cứng", result.Hardness, "-", specs?.Hardness, "Vietaus std", "Shore D");

            Add("Antistatic/Chống tĩnh điện", result.Antistatic, "Có", null, "Vietaus std", "-");
            Add("Black Dots/Chấm đen", result.BlackDots, "-", specs?.BlackDots, "Vietaus std", "-");
            Add("Migration Test/ Di hành màu", result.MigrationTest == true ? "YES" : null, "Đạt", specs?.MigrationTest, "Vietaus std", "-");

            return list;
        }
    }
}

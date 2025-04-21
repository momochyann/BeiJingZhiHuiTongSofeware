using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPOI.SS.UserModel;
using System.IO;
using NPOI.HSSF.UserModel;
using QFramework;
using System;
using System.Reflection;
using NPOI.XWPF.UserModel;
using System.Data;
using NPOI.OpenXmlFormats.Wordprocessing;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class ChineseNameAttribute : Attribute
{
    public string ChineseName { get; }
    public ChineseNameAttribute(string chineseName)
    {
        ChineseName = chineseName;
    }
}
public interface IExcel
{
    Dictionary<string, string> GetchineseName();
}
public abstract class AbstractEXCELData : IExcel
{
    public Dictionary<string, string> GetchineseName()
    {
        var chineseNames = new Dictionary<string, string>();

        // 获取当前类的类型信息
        var type = GetType();

        // 获取所有字段
        foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
        {
            var attribute = field.GetCustomAttribute<ChineseNameAttribute>();
            if (attribute != null)
            {
                chineseNames.Add(field.Name, attribute.ChineseName);
            }
        }
        // 获取所有属性
        foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
        {
            var attribute = property.GetCustomAttribute<ChineseNameAttribute>();
            if (attribute != null)
            {
                chineseNames.Add(property.Name, attribute.ChineseName);
            }
        }
        return chineseNames;
    }
}



public class ExcitExcel : IUtility
{
    public void ExportExcel<T>(List<T> datas, string filePath) where T : AbstractEXCELData
    {
        IWorkbook workbook = new HSSFWorkbook();
        ISheet sheet = workbook.CreateSheet(typeof(T).ToString());
        var varFields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        int index = 0;

        // 创建标题行
        var headname = datas[0].GetchineseName();
        IRow headerRow = sheet.CreateRow(0);

        // 添加导出日期列


        foreach (var field in varFields)
        {
            headerRow.CreateCell(index++).SetCellValue(headname[field.Name]);
        }

        // 填充数据
        for (int i = 0; i < datas.Count; i++)
        {
            T data = datas[i];
            IRow row = sheet.CreateRow(i + 1);
            index = 0; // 从第三列开始填充数据，跳过导出日期列

            foreach (var field in varFields)
            {
                row.CreateCell(index++).SetCellValue(field.GetValue(data)?.ToString());
            }
        }
        headerRow.CreateCell(index++).SetCellValue(DateTime.Now.ToString("yyyy-MM-dd"));
        // string filePath = "/storage/emulated/0/MOMO/班级.xlsx";
        Directory.CreateDirectory(Path.GetDirectoryName(filePath));
        using (FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
        {
            workbook.Write(stream);
        }
        Debug.Log("导出成功");
    }

    // public void ExportWord<T>(List<T> datas, string filePath, studentExcelData studentExcelData, string TrainingName) where T : AbstractEXCELData
    // {
    //     XWPFDocument doc = new XWPFDocument();
    //     try
    //     {
    //         // 创建标题段落
    //         XWPFParagraph titleParagraph = doc.CreateParagraph();
    //         titleParagraph.Alignment = ParagraphAlignment.CENTER;
    //         XWPFRun titleRun = titleParagraph.CreateRun();
    //         titleRun.SetText(TrainingName.Substring(0, 3) + "训练");
    //         titleRun.IsBold = true;
    //         titleRun.FontSize = 16;

    //         // ��建基本信息
    //         XWPFParagraph infoParagraph = doc.CreateParagraph();
    //         XWPFRun infoRun = infoParagraph.CreateRun();
    //         infoRun.SetText($"学校：                班级：{studentExcelData.className}         姓名：{studentExcelData.studentName}");
    //         infoRun.FontSize = 12;

    //         // 添加1个空行
    //         XWPFParagraph emptyParagraph2 = doc.CreateParagraph();
    //         XWPFRun emptyRun2 = emptyParagraph2.CreateRun();
    //         emptyRun2.AddCarriageReturn();

    //         // 计算本周的周一和周日
    //         DateTime today = DateTime.Now;
    //         int daysUntilMonday = ((int)today.DayOfWeek - 1 + 7) % 7;
    //         DateTime currentMonday = today.AddDays(-daysUntilMonday);
    //         DateTime currentSunday = currentMonday.AddDays(6);
    //         // 创建时间信息
    //         XWPFParagraph timeParagraph = doc.CreateParagraph();
    //         XWPFRun timeRun = timeParagraph.CreateRun();
    //         timeRun.SetText($"时间：{currentMonday.Year} 年 {currentMonday.Month} 月 {currentMonday.Day} 日---- {currentSunday.Month} 月 {currentSunday.Day} 日");
    //         timeRun.FontSize = 12;

    //         // 创建训练类型信息
    //         XWPFParagraph trainingParagraph = doc.CreateParagraph();
    //         XWPFRun trainingRun = trainingParagraph.CreateRun();
    //         trainingRun.SetText($"调节训练：{TrainingName[4..]}");
    //         trainingRun.FontSize = 12;


    //         // 创建训练类型信息
    //         XWPFParagraph trainingParagraph2 = doc.CreateParagraph();
    //         XWPFRun trainingRun2 = trainingParagraph2.CreateRun();
    //         trainingRun2.SetText("集合训练：聚散球");
    //         trainingRun2.FontSize = 12;

    //         // 添加1个空行
    //         XWPFParagraph emptyParagraph1 = doc.CreateParagraph();
    //         XWPFRun emptyRun1 = emptyParagraph1.CreateRun();
    //         emptyRun1.AddCarriageReturn();

    //         // 创建表格表头信息
    //         XWPFParagraph trainingTableTitle = doc.CreateParagraph();
    //         XWPFRun trainingTableTitleRun = trainingTableTitle.CreateRun();
    //         trainingTableTitleRun.SetText($"一、{TrainingName}安排：");
    //         trainingTableTitleRun.FontSize = 12;

    //         // 创建表格
    //         XWPFTable table = doc.CreateTable(datas.Count + 1, 6);
    //         table.Width = 5000;

    //         // 设置表头
    //         var fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
    //         var chineseNames = datas[0].GetchineseName();
    //         // string[] headers = new string[] { "时间", "度数选择", "视标选择", "训练结果", "测试结果", "备注日志" };
    //         XWPFTableRow headerRow = table.GetRow(0);
    //         for (int i = 0; i < fields.Length; i++)
    //         {
    //             XWPFParagraph paragraph = headerRow.GetCell(i).AddParagraph();
    //             paragraph.Alignment = ParagraphAlignment.CENTER;
    //             XWPFRun run = paragraph.CreateRun();
    //             run.SetText(chineseNames[fields[i].Name]);
    //             run.IsBold = true;
    //             run.FontSize = 12;
    //         }

    //         // 填充数据
    //         for (int i = 0; i < datas.Count; i++)  // 遍历所有数据行
    //         {
    //             XWPFTableRow dataRow = table.GetRow(i + 1);  // 获取表格中的当前行（i+1是因为第0行是表头）
    //             dataRow.Height = 150;  // 设置当前行的高度为150缇（约7.5磅）
    //             T data = datas[i];     // 获取当前行对应的数据对象
    //             int cellIndex = 0;     // 初始化单元格索引

    //             // 遍历数据对象的所有字段（包括公共、私有和实例字段）
    //             foreach (var field in typeof(T).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
    //             {
    //                 XWPFParagraph paragraph = dataRow.GetCell(cellIndex).AddParagraph();  // 在当前单元格中添加一个新段落
    //                 paragraph.Alignment = ParagraphAlignment.CENTER;  // 设置段落水平居中对齐
    //                 XWPFRun run = paragraph.CreateRun();  // 创建一个文本运行对象（用于设置文本内容和格式）
    //                 run.SetText(field.GetValue(data)?.ToString() ?? "");  // 设置单元格文本内容，如果字段值为null则显示空字符串
    //                 run.FontSize = 9;   // 设置文本字体大小为9磅
    //                 cellIndex++;        // 移动到下一个单元格
    //             }
    //         }

    //         // 合并日期单元格
    //         for (int i = 1; i < datas.Count; i += 2)
    //         {
    //             MergeCellsVertically(table, 0, i, i + 1);
    //         }
    //         Debug.Log(TrainingName.Substring(1, 1));
    //         // 合并双眼翻转拍单元格
    //         if (TrainingName.Substring(1, 1) == "八" || TrainingName.Substring(1, 1) == "九")
    //         {
    //             for (int i = 1; i < datas.Count; i += 2)
    //             {
    //                 MergeCellsVertically(table, 3, i, i + 1);
    //                 MergeCellsVertically(table, 4, i, i + 1);
    //             }
    //         }
    //         Debug.Log("双眼翻转拍单元格合并完成");

    //         //填充老师签名表格
    //         // 记录原始数据最后一行的索引
    //         int lastDataRowIndex = datas.Count;

    //         XWPFTableRow newRow = table.CreateRow();   // 获取表格中的当前行（i+1是因为第0行是表头）
    //         newRow.Height = 500;  // 设置当前行的高度为150缇（约7.5磅）
    //         for (int j = 0; j < fields.Length; j++)
    //         {
    //             XWPFTableCell cell = newRow.GetCell(j);
    //             XWPFParagraph paragraph = cell.AddParagraph();
    //             paragraph.Alignment = ParagraphAlignment.CENTER;
    //             XWPFRun run = paragraph.CreateRun();
    //             run.SetText(""); // 设置为空文本
    //             run.FontSize = 12;
    //         }


    //         //合并老师单元格
    //         MergeCellsHorizontally(table, lastDataRowIndex + 1, 0, 5);
    //         //   MergeCellsVertically(table, 0, lastDataRowIndex + 1, lastDataRowIndex + 5);
    //         Debug.Log("老师单元格合并完成");
    //         XWPFTableCell mergedCell = table.GetRow(lastDataRowIndex + 1).GetCell(0);
    //         // // 创建新段落并设置右对齐
    //         XWPFParagraph _paragraph = mergedCell.GetParagraphArray(0);
    //         _paragraph.Alignment = ParagraphAlignment.RIGHT; // 设置右对齐                      
    //         for (int i = 0; i < 10; i++) // 可以调整数字来控制下移的距离 // 添加空行来将文本下移
    //         {
    //             XWPFRun emptyRun = _paragraph.CreateRun();
    //             emptyRun.AddCarriageReturn();
    //         }
    //         // 添加"指导老师："文本
    //         XWPFRun teachRun = _paragraph.CreateRun();
    //         teachRun.SetText("指导老师：");
    //         teachRun.FontSize = 9;
    //         teachRun.AddTab(); // 添加一个制表符，让文本稍微往左移一点
    //         teachRun.AddTab();
    //         teachRun.AddTab();
    //         mergedCell.SetVerticalAlignment(XWPFTableCell.XWPFVertAlign.BOTTOM);
    //         Debug.Log("添加指导老师");


    //         // // 添加5个空行
    //         // for (int i = 0; i < 5; i++)
    //         // {
    //         //     XWPFParagraph emptyParagraph = doc.CreateParagraph();
    //         //     XWPFRun emptyRun = emptyParagraph.CreateRun();
    //         //     emptyRun.AddCarriageReturn();
    //         // }

    //         // 确保文件扩展名为 .docx
    //         if (!filePath.EndsWith(".docx", StringComparison.OrdinalIgnoreCase))
    //         {
    //             filePath = Path.ChangeExtension(filePath, ".docx");
    //         }

    //         // 保存文件
    //         string directory = Path.GetDirectoryName(filePath);
    //         if (!string.IsNullOrEmpty(directory))
    //         {
    //             Directory.CreateDirectory(directory);
    //         }

    //         using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
    //         {
    //             doc.Write(fs);
    //         }
    //         Debug.Log("导出成功");
    //     }
    //     finally
    //     {
    //         doc.Close();
    //     }
    // }
    // // 垂直合并单元格（合并同一列的单元格）
    // private void MergeCellsVertically(XWPFTable table, int col, int fromRow, int toRow)
    // {
    //     // 获取第一个单元格的内容
    //     string firstCellText = table.GetRow(fromRow).GetCell(col).GetText();

    //     // 设置合并属性
    //     for (int rowIndex = fromRow; rowIndex <= toRow; rowIndex++)
    //     {
    //         XWPFTableCell cell = table.GetRow(rowIndex).GetCell(col);
    //         var tcPr = cell.GetCTTc().AddNewTcPr();
    //         var vMerge = tcPr.AddNewVMerge();

    //         if (rowIndex == fromRow)
    //         {
    //             // 第一个单元格
    //             vMerge.val = ST_Merge.restart;
    //             cell.SetText("");
    //         }
    //         else
    //         {
    //             // 其他单元格
    //             vMerge.val = ST_Merge.@continue;
    //             cell.SetText("");
    //         }
    //     }
    // }
    // 水平合并单元格（合并同一行的单元格）
    // 水平合并单元格（合并同一行的单元格）
    private void MergeCellsHorizontally(XWPFTable table, int row, int fromCol, int toCol)
    {
        XWPFTableCell cell = table.GetRow(row).GetCell(fromCol);
        string cellText = cell.GetText();

        // 设置合并属性
        var tcPr = cell.GetCTTc().AddNewTcPr();
        var gridSpan = tcPr.AddNewGridspan();
        gridSpan.val = (toCol - fromCol + 1).ToString();

        // 移除要合并的其他单元格
        XWPFTableRow tableRow = table.GetRow(row);
        for (int colIndex = toCol; colIndex > fromCol; colIndex--)
        {
            tableRow.RemoveCell(colIndex);
        }

        // 设置合并后单元格的文本
        cell.SetText(cellText);
    }

}

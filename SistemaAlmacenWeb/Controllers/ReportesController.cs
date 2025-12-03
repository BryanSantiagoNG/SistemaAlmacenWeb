using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaAlmacenWeb.Models;
using System.Data;

namespace SistemaAlmacenWeb.Controllers
{
    public class ReportesController : Controller
    {
        private readonly SistemaAlmacenContext _context;

        public ReportesController(SistemaAlmacenContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> FiltrarVentas(DateTime fechaInicio, DateTime fechaFin)
        {
            fechaFin = fechaFin.AddDays(1).AddTicks(-1);

            var ventas = await _context.Facturas
                .Include(f => f.Cliente)
                .Where(f => f.Fecha >= fechaInicio && f.Fecha <= fechaFin)
                .OrderByDescending(f => f.Fecha)
                .Select(f => new
                {
                    folio = f.IdFactura,
                    fecha = f.Fecha.ToString("dd/MM/yyyy HH:mm"),
                    cliente = f.Cliente != null ? f.Cliente.Nombre : "Público General",
                    total = f.Total
                })
                .ToListAsync();

            return Json(ventas);
        }

        [HttpGet]
        public async Task<IActionResult> DescargarExcel(DateTime fechaInicio, DateTime fechaFin)
        {
            fechaFin = fechaFin.AddDays(1).AddTicks(-1);

            var ventas = await _context.Facturas
                .Include(f => f.Cliente)
                .Where(f => f.Fecha >= fechaInicio && f.Fecha <= fechaFin)
                .OrderBy(f => f.Fecha)
                .ToListAsync();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Reporte Ventas");

                worksheet.Cell(1, 1).Value = "Folio";
                worksheet.Cell(1, 2).Value = "Fecha";
                worksheet.Cell(1, 3).Value = "Cliente";
                worksheet.Cell(1, 4).Value = "Total";

                var header = worksheet.Range("A1:D1");
                header.Style.Font.Bold = true;
                header.Style.Fill.BackgroundColor = XLColor.FromHtml("#0d6efd"); 
                header.Style.Font.FontColor = XLColor.White;

                int row = 2;
                foreach (var v in ventas)
                {
                    worksheet.Cell(row, 1).Value = v.IdFactura;
                    worksheet.Cell(row, 2).Value = v.Fecha;
                    worksheet.Cell(row, 3).Value = v.Cliente?.Nombre ?? "Público General";
                    worksheet.Cell(row, 4).Value = v.Total;
                    worksheet.Cell(row, 4).Style.NumberFormat.Format = "$ #,##0.00"; 
                    row++;
                }

                worksheet.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    string nombreArchivo = $"Reporte_Ventas_{DateTime.Now:yyyyMMdd}.xlsx";

                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nombreArchivo);
                }
            }
        }
    }
}
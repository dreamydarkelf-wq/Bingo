using Microsoft.AspNetCore.Mvc;

public class BingoController : Controller
{
    private const int Size = 5;
    private const string SessionKey = "Bingo";

    public IActionResult Index()
    {
        var model = HttpContext.Session.GetObject<BingoModel>(SessionKey);

        if (model == null)
        {
            model = CreateBingo();
            HttpContext.Session.SetObject(SessionKey, model);
        }

        return View(model);
    }

    [HttpPost]
    public IActionResult ToggleCell(int row, int col)
    {
        var model = HttpContext.Session.GetObject<BingoModel>(SessionKey);
        if (model == null) return BadRequest();

        model.Selected[row][col] = !model.Selected[row][col];
        HttpContext.Session.SetObject(SessionKey, model);

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Reset()
    {
        HttpContext.Session.Remove(SessionKey);
        return RedirectToAction(nameof(Index));
    }

    private BingoModel CreateBingo()
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "Data", "items.txt");
        var items = System.IO.File.ReadAllLines(path)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .OrderBy(_ => Guid.NewGuid())
            .Take(Size * Size)
            .ToArray();

        var model = new BingoModel();
        int k = 0;

        for (int i = 0; i < Size; i++)
            for (int j = 0; j < Size; j++)
                model.Items[i][j] = items[k++];

        return model;
    }
}
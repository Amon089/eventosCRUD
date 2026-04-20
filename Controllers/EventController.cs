using amonMVC.Data;
using amonMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace amonMVC.Controllers;

public class EventsController : Controller
{
    private readonly AppDbContext _context;

    public EventsController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Gallery(string? category, string? order)
    {
        var query = _context.Events
            .Where(e => e.Active)
            .AsQueryable();

        if (!string.IsNullOrEmpty(category)) 
            query = query.Where(e => e.Category == category);

        query = order switch
        {
            "date_asc"  => query.OrderBy(e => e.Date),
            "date_desc" => query.OrderByDescending(e => e.Date),
            "name"      => query.OrderBy(e => e.Name),
            _           => query.OrderBy(e => e.Date)
        };

        var categories = await _context.Events
            .Where(e => e.Active)
            .Select(e => e.Category)
            .Distinct()
            .ToListAsync();

        ViewBag.Categories = categories;
        ViewBag.Category   = category;
        ViewBag.Order      = order;

        return View(await query.ToListAsync());
    }

    public async Task<IActionResult> Index()
    {
        var events = await _context.Events
            .OrderByDescending(e => e.CreatedAt)
            .ToListAsync();
        return View(events);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new Eventos_db());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Eventos_db ev)
    {
        if (!ModelState.IsValid)
            return View(ev);

        ev.CreatedAt = DateTime.Now;
        _context.Add(ev);
        await _context.SaveChangesAsync();
        TempData["Message"] = "Event created successfully.";
        return RedirectToAction(nameof(Index));
    }
    
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var ev = await _context.Events.FindAsync(id);
        if (ev == null) return NotFound();
        return View(ev);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Eventos_db ev)
    {
        if (id != ev.Id) return BadRequest();

        if (!ModelState.IsValid)
            return View(ev);

        try
        {
            _context.Update(ev);
            await _context.SaveChangesAsync();
            TempData["Message"] = "Event updated successfully.";
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.Events.AnyAsync(e => e.Id == id))
                return NotFound();
            throw;
        }

        return RedirectToAction(nameof(Index));
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var ev = await _context.Events.FindAsync(id);
        if (ev == null) return NotFound();

        ev.Active = false;
        _context.Update(ev);
        await _context.SaveChangesAsync();

        TempData["Message"] = $"The event \"{ev.Name}\" has been deactivated.";
        return RedirectToAction(nameof(Index));
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeletePermanent(int id)
    {
        var ev = await _context.Events.FindAsync(id);
        if (ev == null) return NotFound();

        _context.Events.Remove(ev);
        await _context.SaveChangesAsync();
        TempData["Message"] = "Event permanently deleted.";
        return RedirectToAction(nameof(Index));
    }
}



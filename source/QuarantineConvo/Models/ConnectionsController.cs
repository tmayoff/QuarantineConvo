using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuarantineConvo.Data;

namespace QuarantineConvo.Models {
    public class ConnectionsController : Controller {
        private readonly QuarantineConvoContext _context;

        public ConnectionsController(QuarantineConvoContext context) {
            _context = context;
        }

        // GET: Connections
        public async Task<IActionResult> Index() {
            return View(await _context.Connection.ToListAsync());
        }

        // GET: Connections/Details/5
        public async Task<IActionResult> Details(int? id) {
            if (id == null) {
                return NotFound();
            }

            var connection = await _context.Connection
                .FirstOrDefaultAsync(m => m.ID == id);
            if (connection == null) {
                return NotFound();
            }

            return View(connection);
        }

        // GET: Connections/Create
        public IActionResult Create() {
            return View();
        }

        // POST: Connections/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID")] Connection connection) {
            if (ModelState.IsValid) {
                _context.Add(connection);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(connection);
        }

        // GET: Connections/Edit/5
        public async Task<IActionResult> Edit(int? id) {
            if (id == null) {
                return NotFound();
            }

            var connection = await _context.Connection.FindAsync(id);
            if (connection == null) {
                return NotFound();
            }
            return View(connection);
        }

        // POST: Connections/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID")] Connection connection) {
            if (id != connection.ID) {
                return NotFound();
            }

            if (ModelState.IsValid) {
                try {
                    _context.Update(connection);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException) {
                    if (!ConnectionExists(connection.ID)) {
                        return NotFound();
                    }
                    else {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(connection);
        }

        // GET: Connections/Delete/5
        public async Task<IActionResult> Delete(int? id) {
            if (id == null) {
                return NotFound();
            }

            var connection = await _context.Connection
                .FirstOrDefaultAsync(m => m.ID == id);
            if (connection == null) {
                return NotFound();
            }

            return View(connection);
        }

        // POST: Connections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            var connection = await _context.Connection.FindAsync(id);
            _context.Connection.Remove(connection);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ConnectionExists(int id) {
            return _context.Connection.Any(e => e.ID == id);
        }
    }
}

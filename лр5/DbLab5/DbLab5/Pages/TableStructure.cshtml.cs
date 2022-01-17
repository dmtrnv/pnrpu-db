using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DbLab5.Data;
using DbLab5.Misc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DbLab5.Pages
{
    public class TableStructure : PageModel
    {
        public IDbContext Context { get; }
        
        public List<FieldModel> Fields { get; set; } = new();

        public TableStructure(IDbContext context)
        {
            Context = context;
        }
        
        public async Task<IActionResult> OnGet(string tableName)
        {
            try
            {
                Fields = await Context.DescribeTableAsync(tableName);
                Context.WorkingTableName = tableName;
            }
            catch
            {
                return RedirectToPage("/Error");
            }
            
            return Page();
        }
        
        public List<string> GetPropertiesNames(FieldModel field)
        {
            var propertiesNames = new List<string>();
            var props = field.GetType().GetProperties();
            
            foreach (var prop in props)
            {
                propertiesNames.Add(prop.Name);
            }
            
            return propertiesNames;
        }
    }
}
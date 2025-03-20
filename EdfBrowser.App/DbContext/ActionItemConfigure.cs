using EdfBrowser.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EdfBrowser.App
{
    internal class ActionItemConfigure : IEntityTypeConfiguration<ActionItem>
    {
        public void Configure(EntityTypeBuilder<ActionItem> builder)
        {
            builder.HasData(
                  new ActionItem("Open File", "Open local edf files") { Id = -1 });
        }
    }
}

using Microsoft.EntityFrameworkCore;
using QuickPulse.Explains.Text;

namespace EntityFramework.Explained.Schema;

public abstract class SchemaPreTestBase
{
    protected LinesReader GetReader(DbContext context)
    {
        var sql = context.Database.GenerateCreateScript();
        return LinesReader.FromText(sql);
    }
}

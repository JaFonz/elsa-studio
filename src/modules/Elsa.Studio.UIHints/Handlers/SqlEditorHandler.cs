using Elsa.Studio.Contracts;
using Elsa.Studio.Models;
using Elsa.Studio.UIHints.Components;
using Microsoft.AspNetCore.Components;

namespace Elsa.Studio.UIHints.Handlers;

public class SqlEditorHandler : IUIHintHandler
{
    public bool GetSupportsUIHint(string uiHint) => uiHint == "sql-editor";
    public string UISyntax => WellKnownSyntaxNames.Literal;

    public RenderFragment DisplayInputEditor(DisplayInputEditorContext context)
    {
        return builder =>
        {
            builder.OpenComponent(0, typeof(Sql));
            builder.AddAttribute(1, nameof(Sql.EditorContext), context);
            builder.CloseComponent();
        };
    }
}
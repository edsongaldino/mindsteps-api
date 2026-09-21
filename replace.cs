using System;
using System.IO;

public class ProgramEmail {
    public static void Main() {
        string file = @"c:\Projects\mindsteps-api\MindSteps.Application\Utils\EmailTemplates.cs";
        string content = File.ReadAllText(file);
        
        string b64 = File.ReadAllText("logo_small_b64.txt");
        
        string oldHeader1 = @"            <!-- Usando a logo oficial do MindSteps, ou um placeholder estiloso -->
            <h2 style=""""color: #008767; margin: 0; font-size: 28px;"""">?? MindSteps</h2>
            <p style=""""color: #888888; font-size: 13px; margin-top: 4px;"""">Cuidar hoje. Construir amanhã.</p>";
            
        string oldHeader2 = @"            <h2 style=""""color: #008767; margin: 0; font-size: 28px;"""">?? MindSteps</h2>
            <p style=""""color: #888888; font-size: 13px; margin-top: 4px;"""">Cuidar hoje. Construir amanhã.</p>";

        string newHeader = string.Format(@"            <!-- Usando a logo oficial do MindSteps (Base64) -->
            <img src=""""data:image/png;base64,{0}"""" alt=""""MindSteps Logo"""" style=""""max-width: 200px; height: auto;"""" />
            <p style=""""color: #888888; font-size: 13px; margin-top: 12px;"""">Um passo de cada vez.</p>", b64);

        content = content.Replace(oldHeader1, newHeader);
        content = content.Replace(oldHeader2, newHeader);
        
        File.WriteAllText(file, content);
    }
}

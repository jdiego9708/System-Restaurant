﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CapaDatos {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.8.1.0")]
    internal sealed partial class ConfigCapaDatos : global::System.Configuration.ApplicationSettingsBase {
        
        private static ConfigCapaDatos defaultInstance = ((ConfigCapaDatos)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new ConfigCapaDatos())));
        
        public static ConfigCapaDatos Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=172;Initial Catalog=Mistico;User ID=sa;Password=*")]
        public string ConexionBD {
            get {
                return ((string)(this["ConexionBD"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Mistico;Integrated Security=Tr" +
            "ue;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationInten" +
            "t=ReadWrite;MultiSubnetFailover=False")]
        public string ConexionLocalBD {
            get {
                return ((string)(this["ConexionLocalBD"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("ConexionLocalBD")]
        public string ConexionActual {
            get {
                return ((string)(this["ConexionActual"]));
            }
            set {
                this["ConexionActual"] = value;
            }
        }
    }
}

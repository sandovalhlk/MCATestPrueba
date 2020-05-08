using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace BusinessLogicMCA
{
    public class IdentityManagerBL
    {
        public enum PasswordTipos
        {
            Extricta,
            Flexible
        }
        public static string GeneratePassword(PasswordTipos tipoPassword = PasswordTipos.Extricta, int maximoCaracteres = 12)
        {
            /*Generate passwords that contained numbers, letters and special characters - and not easily hacked. */
            /*I started with creating three string variables.  This one tells you how many characters the string will contain. */
            int PasswordLength = maximoCaracteres;
            /*This one, is empty for now - but will ultimately hold the finised randomly generated password        */
            string NewPassword = "";
            /*This one tells you which characters are allowed in this new password */
            string allowedChars = "";
            allowedChars = "1,2,3,4,5,6,7,8,9,0";
            allowedChars += "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,";
            allowedChars += "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,";

            if (tipoPassword == PasswordTipos.Extricta)
                allowedChars += "~,!,@,#,$,%,^,&,*,+,?,¿,[,],{,}";

            char[] sep = { ',' };
            string[] arr = allowedChars.Split(sep);

            string IDString = ""; string temp = "";
            Random rand = new Random();

            /*and lastly - loop through the generation process... */
            for (int i = 0; i < PasswordLength; i++)
            {
                temp = arr[rand.Next(0, arr.Length)];
                IDString += temp;
                NewPassword = IDString;
            }

            return NewPassword;
        }

        /// <summary>
        /// Elimina los acentos de una cadena
        /// </summary>
        /// <param name="inputString">Texto con acentos</param>
        /// <returns>Texto sin acentos</returns>
        public static string QuitarAccentos(string inputString)
        {
            Regex a = new Regex("[á|à|ä|â]", RegexOptions.Compiled);
            Regex e = new Regex("[é|è|ë|ê]", RegexOptions.Compiled);
            Regex i = new Regex("[í|ì|ï|î]", RegexOptions.Compiled);
            Regex o = new Regex("[ó|ò|ö|ô]", RegexOptions.Compiled);
            Regex u = new Regex("[ú|ù|ü|û]", RegexOptions.Compiled);
            Regex n = new Regex("[ñ|Ñ]", RegexOptions.Compiled);
            inputString = a.Replace(inputString, "a");
            inputString = e.Replace(inputString, "e");
            inputString = i.Replace(inputString, "i");
            inputString = o.Replace(inputString, "o");
            inputString = u.Replace(inputString, "u");
            inputString = n.Replace(inputString, "n");
            return inputString;
        }






    }
}

using System.Globalization;
using System.Text;

namespace BarcodeMatching
{
    /// <summary>
    /// Barkod / sipariş kodu / ürün adı eşleştirme normalizasyonu.
    /// Eski Form1.NormalizeForMatch ile birebir aynı çıktı (testle kanıtlanır):
    ///   Trim → tr-TR küçük harf → ı/ğ/ü/ş/ö/ç → i/g/u/s/o/c → tüm boşluklar (\s) silinir → '-' ve '/' silinir.
    /// Fark: kültür nesnesi statik cache'lenir ve Regex/çoklu Replace yerine tek geçişli döngü kullanılır.
    /// </summary>
    public static class BarcodeNormalizer
    {
        /// <summary>Paylaşılan tr-TR kültürü (her çağrıda yeni CultureInfo oluşturmamak için).</summary>
        public static readonly CultureInfo TurkishCulture = CultureInfo.GetCultureInfo("tr-TR");

        private static readonly TextInfo TurkishTextInfo = TurkishCulture.TextInfo;

        public static string Normalize(string? val)
        {
            if (string.IsNullOrWhiteSpace(val))
                return "";

            // Not: Trim gereksiz — tüm boşluk karakterleri zaten aşağıda atılıyor
            // (string.Trim ve Regex \s aynı karakter kümesini kullanır: char.IsWhiteSpace).
            string lowered = TurkishTextInfo.ToLower(val);

            StringBuilder? sb = null;
            for (int i = 0; i < lowered.Length; i++)
            {
                char c = lowered[i];
                char mapped;
                bool changed = true;

                switch (c)
                {
                    case 'ı': mapped = 'i'; break;
                    case 'ğ': mapped = 'g'; break;
                    case 'ü': mapped = 'u'; break;
                    case 'ş': mapped = 's'; break;
                    case 'ö': mapped = 'o'; break;
                    case 'ç': mapped = 'c'; break;
                    case '-':
                    case '/':
                        mapped = '\0';
                        break;
                    default:
                        if (char.IsWhiteSpace(c))
                        {
                            mapped = '\0';
                        }
                        else
                        {
                            mapped = c;
                            changed = false;
                        }
                        break;
                }

                if (changed && sb == null)
                {
                    // İlk değişiklikte o ana kadarki (değişmemiş) kısmı kopyala
                    sb = new StringBuilder(lowered.Length);
                    sb.Append(lowered, 0, i);
                }

                if (sb != null && (!changed || mapped != '\0'))
                {
                    sb.Append(mapped);
                }
            }

            return sb == null ? lowered : sb.ToString();
        }
    }
}

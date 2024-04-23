using System.Xml.Serialization;

namespace BuroTime.Models;

public class TcmbCurrency {

	[XmlAttribute("CrossOrder")] public int CrossOrder { get; set; }
	[XmlAttribute("Kod")] public string Code { get; set; }
	[XmlAttribute("CurrencyCode")] public string CurrencyCode { get; set; }
	public int Unit { get; set; }
	[XmlElement("Isim")] public string Name { get; set; }
	public string CurrencyName { get; set; }


	private string forexBuying;
	private string forexSelling;

	[XmlElement("ForexBuying")]
	public string ForexBuying {
		get => string.IsNullOrEmpty(forexBuying) ? "0.00" : forexBuying;
		set => forexBuying = value;
	}

	[XmlElement("ForexSelling")]
	public string ForexSelling {
		get => string.IsNullOrEmpty(forexSelling) ? "0.00" : forexSelling;
		set => forexSelling = value;
	}

	[XmlElement("BanknoteBuying")] public string BanknoteBuying { get; set; }

	[XmlElement("BanknoteSelling")] public string BanknoteSelling { get; set; }

	[XmlElement("CrossRateUSD")] public string CrossRateUSD { get; set; }

	[XmlElement("CrossRateOther")] public string CrossRateOther { get; set; }
}

[XmlRoot("Tarih_Date")]
public class TcmbDate {
	[XmlAttribute("Tarih")] public string Tarih { get; set; }
	[XmlAttribute("Bulten_No")] public string BultenNo { get; set; }
	[XmlElement("Currency")] public List<TcmbCurrency> Currencies { get; set; }
}
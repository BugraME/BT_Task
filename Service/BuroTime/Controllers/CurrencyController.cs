using BuroTime.Middleware;
using BuroTime.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Xml.Serialization;

namespace BuroTime.Controllers;
[ApiController, Route("[controller]/[action]"), Authorize, ServiceFilter<Logger>]
public class CurrencyController(IHttpClientFactory httpClientFactory) : ControllerBase {
	private readonly HttpClient HttpClient = httpClientFactory.CreateClient("tcmb");

	[HttpGet]
	public async Task<IActionResult> GetCurrency(string date, CurrencyType? type) {
		DateTime datetime = DateTime.TryParseExact(date, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate) ? parsedDate : DateTime.Now;
		if (datetime.DayOfWeek == DayOfWeek.Saturday || datetime.DayOfWeek == DayOfWeek.Sunday) return BadRequest("Hafta Sonu kur bilgisi verilemez.");

		TcmbDate tcmbDate = await GetCurrencyByDate(datetime);
		if (type.HasValue) tcmbDate.Currencies = tcmbDate.Currencies.Where(c => c.CurrencyCode == type.ToString()).ToList();
		return Ok(tcmbDate);
	}
	[HttpGet]
	public async Task<IActionResult> CompareDates(string date1, string date2, CurrencyType? type) {
		DateTime datetime1 = DateTime.TryParseExact(date1, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate1) ? parsedDate1 : DateTime.Now;
		if (datetime1.DayOfWeek == DayOfWeek.Saturday || datetime1.DayOfWeek == DayOfWeek.Sunday) return BadRequest("İlk tarih hafta sonuna denk geliyor.");
		TcmbDate tcmbDate1 = await GetCurrencyByDate(datetime1);

		DateTime datetime2 = DateTime.TryParseExact(date2, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate2) ? parsedDate2 : DateTime.Now;
		if (datetime2.DayOfWeek == DayOfWeek.Saturday || datetime2.DayOfWeek == DayOfWeek.Sunday) return BadRequest("İkinci tarih hafta sonuna denk geliyor.");
		TcmbDate tcmbDate2 = await GetCurrencyByDate(datetime2);

		if (type.HasValue) {
			tcmbDate1.Currencies = tcmbDate1.Currencies.Where(c => c.CurrencyCode == type.ToString()).ToList();
			tcmbDate2.Currencies = tcmbDate2.Currencies.Where(c => c.CurrencyCode == type.ToString()).ToList();
		}


		List<object> currencies = [];
		foreach (TcmbCurrency currency1 in tcmbDate1.Currencies) {
			TcmbCurrency currency2 = tcmbDate2.Currencies.FirstOrDefault(c => c.Code == currency1.Code);
			if (currency2 == null) continue;

			decimal buying1 = decimal.Parse(currency1.ForexBuying, CultureInfo.InvariantCulture);
			decimal buying2 = decimal.Parse(currency2.ForexBuying, CultureInfo.InvariantCulture);

			object compareCurrency = new {
				currency1.Code,
				Name = currency1.Name.Trim(),
				ChangeAmount = buying2 > buying1 ? ("+" + (buying2 - buying1)).ToString() : ("-" + (buying1 - buying2)).ToString(),
				ChangeRate = ((buying2 - buying1) / (buying1 == 0 ? 1 : buying1) * 100).ToString("0.00"),
			};
			currencies.Add(compareCurrency);
		}

		return Ok(currencies);
	}


	private async Task<TcmbDate> GetCurrencyByDate(DateTime date) {
		string dateStr = date.Date == DateTime.Now.Date ? "today" : date.ToString("yyyyMM") + "/" + date.ToString("ddMMyyyy");
		HttpResponseMessage response = await HttpClient.GetAsync($"https://www.tcmb.gov.tr/kurlar/{dateStr}.xml");
		response.EnsureSuccessStatusCode();

		string responseText = await response.Content.ReadAsStringAsync();
		XmlSerializer serializer = new(typeof(TcmbDate));
		using TextReader reader = new StringReader(responseText);
		return (TcmbDate)serializer.Deserialize(reader);
	}
}

public enum CurrencyType {
	USD,
	AUD,
	DKK,
	EUR,
	GBP,
	CHF,
	SEK,
	CAD,
	KWD,
	NOK,
	SAR,
	JPY,
	BGN,
	RON,
	RUB,
	IRR,
	CNY,
	PKR,
	QAR,
	KRW,
	AZN,
	AED,
}

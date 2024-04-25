using amorphie.consent.core.DTO;
using amorphie.consent.core.DTO.OpenBanking;
using amorphie.consent.core.DTO.OpenBanking.Event;
using amorphie.consent.core.DTO.OpenBanking.HHS;
using amorphie.consent.core.Model;
using Elastic.CommonSchema;

namespace amorphie.consent.Service.Interface;

public interface IOBEventService
{
    /// <summary>
    /// Do Event process of openbanking.
    /// Save event and eventitem entities into db
    /// Send post message to yos olay-bildirim
    /// </summary>
    /// <param name="consentId">Consent Id</param>
    /// <param name="katilimciBilgisi">Katilimci Bilgisi object of consent data</param>
    /// <param name="eventType">Event Type</param>
    /// <param name="sourceType">Source Type</param>
    /// <param name="sourceNumber"></param>
    /// <returns></returns>
    public Task DoEventProcess(
        string consentId,
        KatilimciBilgisiDto katilimciBilgisi,
        string eventType,
        string sourceType,
        string sourceNumber);

    /// <summary>
    /// BKM event message is send
    /// </summary>
    /// <param name="eventEntity">To be send event entity data</param>
    /// <returns></returns>
    public Task<ApiResult> SendEventToYos(OBEvent eventEntity);

    /// <summary>
    /// Do system event process.
    /// Get yos information from bkm service and save database
    /// </summary>
    /// <param name="systemEventId">To be processed system event id</param>
    /// <returns>System event process result</returns>
    public Task<bool> DoHhsSystemEventProcess(Guid systemEventId);

    public Task<bool> IsSubscsribedForAyrikGkd(string yosKod, string consentType);
}
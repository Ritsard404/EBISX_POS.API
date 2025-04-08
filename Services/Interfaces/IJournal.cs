﻿using EBISX_POS.API.Services.DTO.Journal;

namespace EBISX_POS.API.Services.Interfaces
{
    public interface IJournal
    {
        // 10- totals
        // 0- tenders
        // 3- items
        // 5- Senior/PWD

        //Task<(bool, string)> AddTenderAcountJournal();
        Task<(bool isSuccess, string message)> AddPwdScAccountJournal(AddPwdScAccountJournalDTO journalDTO);
        Task<(bool isSuccess, string message)> UnpostPwdScAccountJournal(long orderId, string oscaNum);

        Task<(bool isSuccess, string message)> AddPwdScJournal(long orderId);
        Task<(bool isSuccess, string message)> AddItemsJournal(long orderId);
        Task<(bool isSuccess, string message)> AddTendersJournal(long orderId);
        Task<(bool isSuccess, string message)> AddTotalsJournal(long orderId);

    }
}

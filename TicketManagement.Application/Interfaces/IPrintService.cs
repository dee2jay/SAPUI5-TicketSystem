namespace TicketManagementSystem.Application.Interfaces;

public interface IPrintService
{
    public Task UploadTicket();
    public Task DownloadTicket();
    public Task PrintTicket();
}
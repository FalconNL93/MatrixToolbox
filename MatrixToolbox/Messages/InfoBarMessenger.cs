using CommunityToolkit.Mvvm.Messaging.Messages;
using MatrixToolbox.Models;

namespace MatrixToolbox.Messages;

public class UpdateInfoBarMessage : RequestMessage<InfoBarModel>
{
    public UpdateInfoBarMessage(InfoBarModel infoBarModel)
    {
        InfoBarModel = infoBarModel;
    }

    public InfoBarModel InfoBarModel { get; }
}
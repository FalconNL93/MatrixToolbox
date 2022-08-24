using CommunityToolkit.Mvvm.Messaging.Messages;
using MatrixToolbox.Models;

namespace MatrixToolbox.Messages;

public class SetUpdateInfoBarMessage : RequestMessage<InfoBarModel>
{
    public InfoBarModel InfoBarModel { get; }

    public SetUpdateInfoBarMessage(InfoBarModel infoBarModel)
    {
        InfoBarModel = infoBarModel;
    }
}
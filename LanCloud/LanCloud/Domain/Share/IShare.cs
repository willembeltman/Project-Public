using LanCloud.Models.Dtos;
using LanCloud.Models.Share.Responses;

namespace LanCloud.Domain.Share
{
    public interface IShare
    {
        IShareStripe[] ShareStripes { get; }

        FileStripeDto[] ListFileBits();
    }
}
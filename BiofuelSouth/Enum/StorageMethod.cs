using System.ComponentModel.DataAnnotations;

namespace BiofuelSouth.Enum
{
    public enum StorageMethod
    {
		[Display( Name = @"Round Tarp Pallet" )]
		RoundTarpPallet = 1,
		[Display( Name = @"Round Tarp Gravel" )]
		RoundTarpGravel =2,
		[Display( Name = @"Round Tarp Bareground" )]
		RoundTarpBareGround = 3,
		[Display( Name = @"Round Pallet No tarp" )]
		RoundPalletNoTarp =4,
		[Display( Name = @"Round Gravel No tarp" )]
		RoundGravelNoTarp =5,
		[Display( Name = @"Round Bareground NoTarp" )]
		RoundBareGroundNoTarp =6,
		[Display( Name = @"Rectangular Tarp Pallet" )]
		RectangularTarpPallet =11,
		[Display( Name = @"Rectangular Tarp Gravel" )]
		RectangularTarpGravel =12,
		[Display( Name = @"Rectangular No tarp" )]
		RectangularNoTarp =13,
		[Display( Name = @"Rectangular Gravel No tarp" )]
		RectangularGravelNoTarp =14
    }
}
// by Freya Holmér (https://github.com/FreyaHolmer/Mathfs)
// Do not manually edit - this file is generated by MathfsCodegen.cs

using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Freya {

	/// <summary>An optimized uniform 1D Cubic hermite segment, with 4 control points</summary>
	[Serializable] public struct HermiteCubic1D : IParamSplineSegment<Polynomial,Matrix4x1> {

		const MethodImplOptions INLINE = MethodImplOptions.AggressiveInlining;

		/// <summary>Creates a uniform 1D Cubic hermite segment, from 4 control points</summary>
		/// <param name="p0">The starting point of the curve</param>
		/// <param name="v0">The rate of change (velocity) at the start of the curve</param>
		/// <param name="p1">The end point of the curve</param>
		/// <param name="v1">The rate of change (velocity) at the end of the curve</param>
		public HermiteCubic1D( float p0, float v0, float p1, float v1 ) {
			pointMatrix = new Matrix4x1( p0, v0, p1, v1 );
			validCoefficients = false;
			curve = default;
		}

		Polynomial curve;
		public Polynomial Curve {
			get {
				ReadyCoefficients();
				return curve;
			}
		}
		#region Control Points

		[SerializeField] Matrix4x1 pointMatrix;
		public Matrix4x1 PointMatrix {
			get => pointMatrix;
			set => _ = ( pointMatrix = value, validCoefficients = false );
		}

		/// <summary>The starting point of the curve</summary>
		public float P0 {
			[MethodImpl( INLINE )] get => pointMatrix.m0;
			[MethodImpl( INLINE )] set => _ = ( pointMatrix.m0 = value, validCoefficients = false );
		}

		/// <summary>The rate of change (velocity) at the start of the curve</summary>
		public float V0 {
			[MethodImpl( INLINE )] get => pointMatrix.m1;
			[MethodImpl( INLINE )] set => _ = ( pointMatrix.m1 = value, validCoefficients = false );
		}

		/// <summary>The end point of the curve</summary>
		public float P1 {
			[MethodImpl( INLINE )] get => pointMatrix.m2;
			[MethodImpl( INLINE )] set => _ = ( pointMatrix.m2 = value, validCoefficients = false );
		}

		/// <summary>The rate of change (velocity) at the end of the curve</summary>
		public float V1 {
			[MethodImpl( INLINE )] get => pointMatrix.m3;
			[MethodImpl( INLINE )] set => _ = ( pointMatrix.m3 = value, validCoefficients = false );
		}

		/// <summary>Get or set a control point position by index. Valid indices from 0 to 3</summary>
		public float this[ int i ] {
			get =>
				i switch {
					0 => P0,
					1 => V0,
					2 => P1,
					3 => V1,
					_ => throw new ArgumentOutOfRangeException( nameof(i), $"Index has to be in the 0 to 3 range, and I think {i} is outside that range you know" )
				};
			set {
				switch( i ) {
					case 0:
						P0 = value;
						break;
					case 1:
						V0 = value;
						break;
					case 2:
						P1 = value;
						break;
					case 3:
						V1 = value;
						break;
					default: throw new ArgumentOutOfRangeException( nameof(i), $"Index has to be in the 0 to 3 range, and I think {i} is outside that range you know" );
				}
			}
		}

		#endregion
		[NonSerialized] bool validCoefficients;

		[MethodImpl( INLINE )] void ReadyCoefficients() {
			if( validCoefficients )
				return; // no need to update
			validCoefficients = true;
			curve = new Polynomial(
				P0,
				V0,
				-3*P0-2*V0+3*P1-V1,
				2*P0+V0-2*P1+V1
			);
		}
		public static bool operator ==( HermiteCubic1D a, HermiteCubic1D b ) => a.pointMatrix == b.pointMatrix;
		public static bool operator !=( HermiteCubic1D a, HermiteCubic1D b ) => !( a == b );
		public bool Equals( HermiteCubic1D other ) => P0.Equals( other.P0 ) && V0.Equals( other.V0 ) && P1.Equals( other.P1 ) && V1.Equals( other.V1 );
		public override bool Equals( object obj ) => obj is HermiteCubic1D other && pointMatrix.Equals( other.pointMatrix );
		public override int GetHashCode() => pointMatrix.GetHashCode();
		public override string ToString() => $"({pointMatrix.m0}, {pointMatrix.m1}, {pointMatrix.m2}, {pointMatrix.m3})";

		public static explicit operator BezierCubic1D( HermiteCubic1D s ) =>
			new BezierCubic1D(
				s.P0,
				s.P0+(1/3f)*s.V0,
				s.P1-(1/3f)*s.V1,
				s.P1
			);
		public static explicit operator CatRomCubic1D( HermiteCubic1D s ) =>
			new CatRomCubic1D(
				-2*s.V0+s.P1,
				s.P0,
				s.P1,
				s.P0+2*s.V1
			);
		public static explicit operator UBSCubic1D( HermiteCubic1D s ) =>
			new UBSCubic1D(
				-s.P0-(7/3f)*s.V0+2*s.P1-(2/3f)*s.V1,
				2*s.P0+(2/3f)*s.V0-s.P1+(1/3f)*s.V1,
				-s.P0-(1/3f)*s.V0+2*s.P1-(2/3f)*s.V1,
				2*s.P0+(2/3f)*s.V0-s.P1+(7/3f)*s.V1
			);
		/// <summary>Returns a linear blend between two hermite curves</summary>
		/// <param name="a">The first spline segment</param>
		/// <param name="b">The second spline segment</param>
		/// <param name="t">A value from 0 to 1 to blend between <c>a</c> and <c>b</c></param>
		public static HermiteCubic1D Lerp( HermiteCubic1D a, HermiteCubic1D b, float t ) =>
			new(
				Mathfs.Lerp( a.P0, b.P0, t ),
				Mathfs.Lerp( a.V0, b.V0, t ),
				Mathfs.Lerp( a.P1, b.P1, t ),
				Mathfs.Lerp( a.V1, b.V1, t )
			);
	}
}

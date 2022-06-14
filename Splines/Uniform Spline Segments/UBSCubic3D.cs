// by Freya Holmér (https://github.com/FreyaHolmer/Mathfs)
// Do not manually edit - this file is generated by MathfsCodegen.cs

using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Freya {

	/// <summary>An optimized uniform 3D Cubic b-spline segment, with 4 control points</summary>
	[Serializable] public struct UBSCubic3D : IParamSplineSegment<Polynomial3D,Vector3Matrix4x1> {

		const MethodImplOptions INLINE = MethodImplOptions.AggressiveInlining;

		/// <summary>Creates a uniform 3D Cubic b-spline segment, from 4 control points</summary>
		/// <param name="p0">The first point of the B-spline hull</param>
		/// <param name="p1">The second point of the B-spline hull</param>
		/// <param name="p2">The third point of the B-spline hull</param>
		/// <param name="p3">The fourth point of the B-spline hull</param>
		public UBSCubic3D( Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3 ) {
			pointMatrix = new Vector3Matrix4x1( p0, p1, p2, p3 );
			validCoefficients = false;
			curve = default;
		}

		Polynomial3D curve;
		public Polynomial3D Curve {
			get {
				ReadyCoefficients();
				return curve;
			}
		}
		#region Control Points

		[SerializeField] Vector3Matrix4x1 pointMatrix;
		public Vector3Matrix4x1 PointMatrix {
			get => pointMatrix;
			set => _ = ( pointMatrix = value, validCoefficients = false );
		}

		/// <summary>The first point of the B-spline hull</summary>
		public Vector3 P0 {
			[MethodImpl( INLINE )] get => pointMatrix.m0;
			[MethodImpl( INLINE )] set => _ = ( pointMatrix.m0 = value, validCoefficients = false );
		}

		/// <summary>The second point of the B-spline hull</summary>
		public Vector3 P1 {
			[MethodImpl( INLINE )] get => pointMatrix.m1;
			[MethodImpl( INLINE )] set => _ = ( pointMatrix.m1 = value, validCoefficients = false );
		}

		/// <summary>The third point of the B-spline hull</summary>
		public Vector3 P2 {
			[MethodImpl( INLINE )] get => pointMatrix.m2;
			[MethodImpl( INLINE )] set => _ = ( pointMatrix.m2 = value, validCoefficients = false );
		}

		/// <summary>The fourth point of the B-spline hull</summary>
		public Vector3 P3 {
			[MethodImpl( INLINE )] get => pointMatrix.m3;
			[MethodImpl( INLINE )] set => _ = ( pointMatrix.m3 = value, validCoefficients = false );
		}

		/// <summary>Get or set a control point position by index. Valid indices from 0 to 3</summary>
		public Vector3 this[ int i ] {
			get =>
				i switch {
					0 => P0,
					1 => P1,
					2 => P2,
					3 => P3,
					_ => throw new ArgumentOutOfRangeException( nameof(i), $"Index has to be in the 0 to 3 range, and I think {i} is outside that range you know" )
				};
			set {
				switch( i ) {
					case 0:
						P0 = value;
						break;
					case 1:
						P1 = value;
						break;
					case 2:
						P2 = value;
						break;
					case 3:
						P3 = value;
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
			curve = new Polynomial3D(
				(1/6f)*P0+(2/3f)*P1+(1/6f)*P2,
				(-P0+P2)/2,
				(1/2f)*P0-P1+(1/2f)*P2,
				-(1/6f)*P0+(1/2f)*P1-(1/2f)*P2+(1/6f)*P3
			);
		}
		public static bool operator ==( UBSCubic3D a, UBSCubic3D b ) => a.pointMatrix == b.pointMatrix;
		public static bool operator !=( UBSCubic3D a, UBSCubic3D b ) => !( a == b );
		public bool Equals( UBSCubic3D other ) => P0.Equals( other.P0 ) && P1.Equals( other.P1 ) && P2.Equals( other.P2 ) && P3.Equals( other.P3 );
		public override bool Equals( object obj ) => obj is UBSCubic3D other && pointMatrix.Equals( other.pointMatrix );
		public override int GetHashCode() => pointMatrix.GetHashCode();
		public override string ToString() => $"({pointMatrix.m0}, {pointMatrix.m1}, {pointMatrix.m2}, {pointMatrix.m3})";

		/// <summary>Returns this curve flattened to 2D. Effectively setting z = 0</summary>
		/// <param name="curve3D">The 3D curve to flatten to the Z plane</param>
		public static explicit operator UBSCubic2D( UBSCubic3D curve3D ) => new UBSCubic2D( curve3D.P0, curve3D.P1, curve3D.P2, curve3D.P3 );
		public static explicit operator BezierCubic3D( UBSCubic3D s ) =>
			new BezierCubic3D(
				(1/6f)*s.P0+(2/3f)*s.P1+(1/6f)*s.P2,
				(2/3f)*s.P1+(1/3f)*s.P2,
				(1/3f)*s.P1+(2/3f)*s.P2,
				(1/6f)*s.P1+(2/3f)*s.P2+(1/6f)*s.P3
			);
		public static explicit operator HermiteCubic3D( UBSCubic3D s ) =>
			new HermiteCubic3D(
				(1/6f)*s.P0+(2/3f)*s.P1+(1/6f)*s.P2,
				(-s.P0+s.P2)/2,
				(1/6f)*s.P1+(2/3f)*s.P2+(1/6f)*s.P3,
				(-s.P1+s.P3)/2
			);
		public static explicit operator CatRomCubic3D( UBSCubic3D s ) =>
			new CatRomCubic3D(
				s.P0+(1/6f)*s.P1-(1/3f)*s.P2+(1/6f)*s.P3,
				(1/6f)*s.P0+(2/3f)*s.P1+(1/6f)*s.P2,
				(1/6f)*s.P1+(2/3f)*s.P2+(1/6f)*s.P3,
				(1/6f)*s.P0-(1/3f)*s.P1+(1/6f)*s.P2+s.P3
			);
		/// <summary>Returns a linear blend between two b-spline curves</summary>
		/// <param name="a">The first spline segment</param>
		/// <param name="b">The second spline segment</param>
		/// <param name="t">A value from 0 to 1 to blend between <c>a</c> and <c>b</c></param>
		public static UBSCubic3D Lerp( UBSCubic3D a, UBSCubic3D b, float t ) =>
			new(
				Vector3.LerpUnclamped( a.P0, b.P0, t ),
				Vector3.LerpUnclamped( a.P1, b.P1, t ),
				Vector3.LerpUnclamped( a.P2, b.P2, t ),
				Vector3.LerpUnclamped( a.P3, b.P3, t )
			);
	}
}

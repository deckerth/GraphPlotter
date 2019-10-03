Namespace Global.MathEvaluatorTest.Domains

    Public Class Vector

        Public Property X As Double = 0.0
        Public Property Y As Double = 0.0
        Public Property Z As Double = 0.0

        Private Shared _zero = New Vector(0, 0, 0)
        Public Shared ReadOnly Property Zero As Vector
            Get
                Return _zero
            End Get
        End Property

        Private Shared _one = New Vector(1, 1, 1)
        Public Shared ReadOnly Property One As Vector
            Get
                Return _one
            End Get
        End Property

        Public Sub New()
        End Sub

        Public Sub New(xVal As Double, yVal As Double, zVal As Double)
            X = xVal
            Y = yVal
            Z = zVal
        End Sub

        Public Function Scale(a As Double) As Vector
            Return New Vector(X * a, Y * a, Z * a)
        End Function

        Public Function Add(v As Vector) As Vector
            Return New Vector(X + v.X, Y + v.Y, Z + v.Z)
        End Function

        Public Function MultScalar(v As Vector) As Vector
            Return New Vector(X * v.X, Y * v.Y, Z * v.Z)
        End Function

        Public Function MultCross(v As Vector) As Vector
            Return New Vector(Y * v.Z - Z * v.Y, Y * v.X - X * v.Z, X * v.Y - Y * v.X)
        End Function

        Public Function Length() As Double
            Return Math.Sqrt(X * X + Y * Y + Z * Z)
        End Function


    End Class

End Namespace


using NUnit.Framework;
using Unity.PerformanceTesting;

public class DemoTest {
	[Test, Performance]
	public void UnityMathfAbs() {
		Measure.Method (() => {
			UnityEngine.Mathf.Abs (-1);
		})
		.WarmupCount (16)
		.IterationsPerMeasurement (10000000)
		.MeasurementCount (16)
		.Run();
	}

	[Test, Performance]
	public void SystemMathAbs() {
		Measure.Method (() => {
			System.Math.Abs (-1);
		})
		.WarmupCount (16)
		.IterationsPerMeasurement (10000000)
		.MeasurementCount (16)
		.Run();
	}
}

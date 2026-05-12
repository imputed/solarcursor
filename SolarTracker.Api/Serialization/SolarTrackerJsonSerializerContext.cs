using System.Text.Json;
using System.Text.Json.Serialization;
using SolarTracker.Application.Analysis.Common;
using SolarTracker.Application.Analysis.CurrentMeasuringUnit;
using SolarTracker.Application.Analysis.InstallationSite;
using SolarTracker.Application.Analysis.LinearMotor;
using SolarTracker.Application.Analysis.SolarPanel;
using SolarTracker.Application.Analysis.TiltMeasuringUnit;
using SolarTracker.Application.Dtos.CurrentMeasuringUnit;
using SolarTracker.Application.Dtos.InstallationSite;
using SolarTracker.Application.Dtos.LinearMotor;
using SolarTracker.Application.Dtos.SolarOptimizationScheduleConfiguration;
using SolarTracker.Application.Dtos.SolarPanel;
using SolarTracker.Application.Dtos.SolarPanelOptimizationState;
using SolarTracker.Application.Dtos.SolarTrackingConfiguration;
using SolarTracker.Application.Dtos.TiltMeasuringUnit;

namespace SolarTracker.Api.Serialization;

[JsonSourceGenerationOptions(JsonSerializerDefaults.Web)]
[JsonSerializable(typeof(InstallationSiteDto))]
[JsonSerializable(typeof(CreateInstallationSiteDto))]
[JsonSerializable(typeof(UpdateInstallationSiteDto))]
[JsonSerializable(typeof(IReadOnlyList<InstallationSiteDto>))]
[JsonSerializable(typeof(List<InstallationSiteDto>))]
[JsonSerializable(typeof(SolarPanelDto))]
[JsonSerializable(typeof(SolarPanelOptimizationStateDto))]
[JsonSerializable(typeof(CreateSolarPanelDto))]
[JsonSerializable(typeof(UpdateSolarPanelOptimizationStateDto))]
[JsonSerializable(typeof(UpdateSolarPanelDto))]
[JsonSerializable(typeof(IReadOnlyList<SolarPanelDto>))]
[JsonSerializable(typeof(List<SolarPanelDto>))]
[JsonSerializable(typeof(SolarOptimizationScheduleConfigurationDto))]
[JsonSerializable(typeof(UpdateSolarOptimizationScheduleConfigurationDto))]
[JsonSerializable(typeof(SolarTrackingConfigurationDto))]
[JsonSerializable(typeof(UpdateSolarTrackingConfigurationDto))]
[JsonSerializable(typeof(LinearMotorDto))]
[JsonSerializable(typeof(CreateLinearMotorDto))]
[JsonSerializable(typeof(UpdateLinearMotorDto))]
[JsonSerializable(typeof(IReadOnlyList<LinearMotorDto>))]
[JsonSerializable(typeof(List<LinearMotorDto>))]
[JsonSerializable(typeof(TiltMeasuringUnitDto))]
[JsonSerializable(typeof(CreateTiltMeasuringUnitDto))]
[JsonSerializable(typeof(UpdateTiltMeasuringUnitDto))]
[JsonSerializable(typeof(IReadOnlyList<TiltMeasuringUnitDto>))]
[JsonSerializable(typeof(List<TiltMeasuringUnitDto>))]
[JsonSerializable(typeof(CurrentMeasuringUnitDto))]
[JsonSerializable(typeof(CreateCurrentMeasuringUnitDto))]
[JsonSerializable(typeof(UpdateCurrentMeasuringUnitDto))]
[JsonSerializable(typeof(IReadOnlyList<CurrentMeasuringUnitDto>))]
[JsonSerializable(typeof(List<CurrentMeasuringUnitDto>))]
[JsonSerializable(typeof(InstallationSiteAnalyzeRequest))]
[JsonSerializable(typeof(InstallationSiteAnalysisNode))]
[JsonSerializable(typeof(InstallationSiteAllNode))]
[JsonSerializable(typeof(InstallationSiteAnyNode))]
[JsonSerializable(typeof(InstallationSiteLeafPredicate))]
[JsonSerializable(typeof(InstallationSiteAnalyzeField))]
[JsonSerializable(typeof(SolarPanelAnalyzeRequest))]
[JsonSerializable(typeof(SolarPanelAnalysisNode))]
[JsonSerializable(typeof(SolarPanelAllNode))]
[JsonSerializable(typeof(SolarPanelAnyNode))]
[JsonSerializable(typeof(SolarPanelLeafPredicate))]
[JsonSerializable(typeof(SolarPanelAnalyzeField))]
[JsonSerializable(typeof(LinearMotorAnalyzeRequest))]
[JsonSerializable(typeof(LinearMotorAnalysisNode))]
[JsonSerializable(typeof(LinearMotorAllNode))]
[JsonSerializable(typeof(LinearMotorAnyNode))]
[JsonSerializable(typeof(LinearMotorLeafPredicate))]
[JsonSerializable(typeof(LinearMotorAnalyzeField))]
[JsonSerializable(typeof(TiltMeasuringUnitAnalyzeRequest))]
[JsonSerializable(typeof(TiltMeasuringUnitAnalysisNode))]
[JsonSerializable(typeof(TiltMeasuringUnitAllNode))]
[JsonSerializable(typeof(TiltMeasuringUnitAnyNode))]
[JsonSerializable(typeof(TiltMeasuringUnitLeafPredicate))]
[JsonSerializable(typeof(TiltMeasuringUnitAnalyzeField))]
[JsonSerializable(typeof(CurrentMeasuringUnitAnalyzeRequest))]
[JsonSerializable(typeof(CurrentMeasuringUnitAnalysisNode))]
[JsonSerializable(typeof(CurrentMeasuringUnitAllNode))]
[JsonSerializable(typeof(CurrentMeasuringUnitAnyNode))]
[JsonSerializable(typeof(CurrentMeasuringUnitLeafPredicate))]
[JsonSerializable(typeof(CurrentMeasuringUnitAnalyzeField))]
[JsonSerializable(typeof(ScalarComparisonOperator))]
internal sealed partial class SolarTrackerJsonSerializerContext : JsonSerializerContext;

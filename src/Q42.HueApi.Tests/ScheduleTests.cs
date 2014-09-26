﻿using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using Q42.HueApi.Interfaces;
using Q42.HueApi.Models;
using System.Globalization;

namespace Q42.HueApi.Tests
{
  [TestClass]
  public class ScheduleTests
  {
    private IHueClient _client;

    [TestInitialize]
    public void Initialize()
    {
      string ip = ConfigurationManager.AppSettings["ip"].ToString();
      string key = ConfigurationManager.AppSettings["key"].ToString();

      _client = new HueClient(ip, key);
    }

    [TestMethod]
    public async Task GetAll()
    {
      var result = await _client.GetSchedulesAsync();

      Assert.AreNotEqual(0, result.Count);
    }

    [TestMethod]
    public async Task GetSingle()
    {
      var all = await _client.GetSchedulesAsync();

      Assert.IsNotNull(all);
      Assert.IsTrue(all.Any());

      var single = await _client.GetScheduleAsync(all.First().Id);

      Assert.IsNotNull(single);
    }

    [TestMethod]
    public async Task CreateScheduleSingle()
    {
      Schedule schedule = new Schedule();
      schedule.Name = "t1";
      schedule.Description = "test";
      schedule.Time = DateTime.Now.AddDays(1);
      schedule.Command = new ScheduleCommand();
      schedule.Command.Body = new LightCommand();
      schedule.Command.Body.Alert = Alert.Once;
      schedule.Command.Address = "/api/huelandspoor/lights/5/state";
      schedule.Command.Method = "PUT";

      var result = await _client.CreateScheduleAsync(schedule);

      Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task UpdateSchedule()
    {
      Schedule schedule = new Schedule();
      schedule.Name = "t1";
      schedule.Description = "test";
      schedule.Time = DateTime.UtcNow.AddDays(1);
      schedule.Command = new ScheduleCommand();
      schedule.Command.Body = new LightCommand();
      schedule.Command.Body.Alert = Alert.Once;
      schedule.Command.Address = "/api/huelandspoor/lights/5/state";
      schedule.Command.Method = "PUT";

      var scheduleId = await _client.CreateScheduleAsync(schedule);

      //Update name
      schedule.Name = "t2";
      await _client.UpdateScheduleAsync(scheduleId, schedule);

      //Get saved schedule
      var savedSchedule = await _client.GetScheduleAsync(scheduleId);

      //Check 
      Assert.AreEqual(schedule.Name, savedSchedule.Name);

    }

    [TestMethod]
    public async Task DeleteSchedule()
    {
      Schedule schedule = new Schedule();
      schedule.Name = "t1";
      schedule.Description = "test";
      schedule.Time = DateTime.UtcNow.AddDays(1);
      schedule.Command = new ScheduleCommand();
      schedule.Command.Body = new LightCommand();
      schedule.Command.Body.Alert = Alert.Once;
      schedule.Command.Address = "/api/huelandspoor/lights/5/state";
      schedule.Command.Method = "PUT";

      var scheduleId = await _client.CreateScheduleAsync(schedule);

      //Delete
      await _client.DeleteScheduleAsync(scheduleId);

    }
   
   
  }
}

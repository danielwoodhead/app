﻿using System;

namespace MyHealth.Web.App.Areas.HealthRecord.Models
{
    public class Observation
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string Value { get; set; }
        public DateTimeOffset DateTime { get; set; }
    }
}

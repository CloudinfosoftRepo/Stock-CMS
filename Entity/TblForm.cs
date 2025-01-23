using System;
using System.Collections.Generic;

namespace Stock_CMS.Entity;

public partial class TblForm
{
    public int Id { get; set; }

    public string? FormName { get; set; }

    public string? Url { get; set; }

    public int? Sequence { get; set; }
}

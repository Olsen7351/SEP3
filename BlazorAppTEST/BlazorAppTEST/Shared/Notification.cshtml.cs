﻿@if (TempData["success"] != null)
{
    <script src="~/lib/jquery/dist/jquery.min.js"></script>
        <script src="//cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/js/toastr.min.js"></script>
        <script type="text/javascript">
                     toastr.success('@TempData["success"]');
        </script>
}

@if (TempData["error"] != null)
{

    <script src="~/lib/jquery/dist/jquery.min.js"></script>
        <script src="//cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/js/toastr.min.js"></script>
        <script type="text/javascript">
                     toastr.error('@TempData["success"]');
        </script>
}


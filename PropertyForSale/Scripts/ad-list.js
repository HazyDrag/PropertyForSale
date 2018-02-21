$(document).ready(function () {

    if ($('#dataswitcher').first().attr('data-type') == 'mylist') {
        GetAdverts('1', true);
    }
    else {
        GetAdverts('1', false);
    }
        
    function GetAdverts(page, isUserList) {
        var routeValues = { page: page, isUserList: isUserList };
        $.get('/Advert/GetAdList', routeValues, function (data) {
            CreatePaginator(data.PagingInfo.TotalPages, data.PagingInfo.CurrentPage);

            var adverts = data.Adverts;
            //if author that advert not you then delete ID author and not create edit btn
            $.each(adverts, function (key, value) {
                if (value.User.ID != data.MyId) {
                    value.User.ID = null;
                }
            });

            //TEMPLATE USE
            var templateWithData = Mustache.to_html($("#advertTemplate").html(), { RootTag: adverts });
            $("#advertContainer").empty().html(templateWithData);
        });
    }

    function CreatePaginator(pages, currentPage) {
        $("#paginatorContainer").empty();

        if (pages > 5) {
            AppendButton('1');
            if (currentPage > 3) {
                var dots1 = $("<a></a>").text('...').addClass('btn btn-nav-dots');
                $("#paginatorContainer").append(dots1);
            }
            if (currentPage > 2) {
                AppendButton(currentPage - 1);
            }
            if (currentPage != 1 && currentPage != pages) {
                AppendButton(currentPage);
            }
            if (currentPage < pages - 1) {
                AppendButton(currentPage + 1);
            }
            if (currentPage < pages - 2) {
                var dots2 = $("<a></a>").text('...').addClass('btn btn-nav-dots');
                $("#paginatorContainer").append(dots2);
            }
            AppendButton(pages);
        }
        else {
            for (var i = 1; i <= pages; i++) {
                AppendButton(i);
            }
        }
        $("#btn-nav-" + currentPage).addClass('btn-primary');
        $('.btn-nav').click(NavigationClick);
    }

    function AppendButton(id) {
        var button = $("<a></a>").addClass('btn btn-default btn-nav').text(id).attr('id', 'btn-nav-' + id);
        $("#paginatorContainer").append(button);
    }

    function NavigationClick() {
        if ($('#dataswitcher').first().attr('data-type') == 'mylist') {
            GetAdverts($(this).text(), true);
        }
        else {
            GetAdverts($(this).text(), false);
        }
    }
});
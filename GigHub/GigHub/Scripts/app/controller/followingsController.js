var FollowingsController = function (followingService) {
    var button;

    var init = function () {
        $(".js-toggle-follow").click(toggleFollowing);
    };


    var toggleFollowing = function (e) {
        button = $(e.target);

        var followeeId = button.attr("data-user-id");

        if (button.hasClass("btn-secondary"))
            followingService.followArtist(followeeId, done, fail);
        else
            followingService.unfollowArtist(followeeId, done, fail);
    };

    var done = function () {
        var text = (button.text() == "Following") ? "Follow" : "Following";
        button
            .toggleClass("btn-info")
            .toggleClass("btn-secondary")
            .text(text);
    };

    var fail = function () {
        alert("Something failed!");
    };

    return {
        init: init
    }

}(FollowingService);
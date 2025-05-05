$(() => {

    $('#like-button').on('click', function () {
        const id = $('.row').data('id');
        $.post('/home/like', { id: imageId }, function () {
            $('#like-button').prop('disabled', true);
        });
    });

    setInterval(function () {
        const id = $('.row').data('id');
        $.get('/home/getlikes', { id: imageId }, function (likes) {
            $('#likes-count').text(likes);
            console.log(likes)
        });
    }, 1000);
});

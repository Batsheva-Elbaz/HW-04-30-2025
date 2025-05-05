$(() => {
    const imageId = $('#image-id').val();

    $('#like-button').on('click', function () {
        $.post('/home/like', { id: imageId }, function (result) {
            $('#likes-count').text(result.likes);
            $('#like-button').prop('disabled', true);
        });
    });

    setInterval(function () {
        $.get('/home/getlikes', { id: imageId }, function (like) {
            $('#likes-count').text(like.likes);
        });
    }, 1000);
});

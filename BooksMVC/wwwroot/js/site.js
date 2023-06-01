$('#CoverPick').change(() => {
    $('#BookCover').attr('src', URL.createObjectURL($('#CoverPick').prop('files')[0]));
});

$('#editbtn').click(async () => {
    const dt = new DataTransfer();
    let path = /base64,.+/.exec($('#BookCover').attr('src'))[0];
    path = path.substring(2, path.length - 1);
    dt.items.add(base64toBlob(path, 'image/png'));
    const file_list = dt.files;
    document.querySelector('#CoverPick').files = file_list;
});
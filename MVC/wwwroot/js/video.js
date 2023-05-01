function DeleteVideoWarning(url) {
    Swal.fire({
        title: 'Are you sure you want delete this video?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Delete'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'Delete',
                success: function (data) {
                    if (data.success) {
                        Swal.fire(
                            'Deleted!',
                            'Video has been deleted successfully',
                            'success'
                        );

                        $('#tblVideos').load(window.location.href + ' #tblVideos');
                    } else {
                        Swal.fire({
                            icon: 'error',
                            title: 'Oops...',
                            text: 'SUnable to delete video!'
                        })
                    }
                }
            })
        }
    })
}
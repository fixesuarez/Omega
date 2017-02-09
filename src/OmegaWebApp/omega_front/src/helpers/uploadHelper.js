import $ from 'jquery'


function dataFilter(data, type) {
    if(data === '') return null;
    return data;
}

export async function uploadAsync(endpoint, id, token, data) {
    return await $.ajax({
        url: endpoint.concat('/', id),
        data: data,
        processData: false,
        contentType: false,
        type: 'POST',
        headers: {
            Authorization: `Bearer ${token}`
        },
        success: function(data){
        }
      });
}
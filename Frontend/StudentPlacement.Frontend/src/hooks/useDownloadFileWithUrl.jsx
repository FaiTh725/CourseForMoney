
const useDownloadFileWithUrl = (url, name) => {

    var link = document.createElement('a');
    link.href = url;
    link.setAttribute("download", name);
    document.body.appendChild(link);
    link.click();

    document.body.removeChild(link);
}

export default useDownloadFileWithUrl;
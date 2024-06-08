
const useDownloadFileOnClick = (url, name) => {
    var href = URL.createObjectURL(url);

    var link = document.createElement('a');
    link.href = href;
    link.setAttribute("download", name);
    document.body.appendChild(link);
    link.click();

    document.body.removeChild(link);
    URL.revokeObjectURL(href);
}

export default useDownloadFileOnClick;
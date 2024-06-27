var map = null; // 地図データ
// jsonファイル読込
let coordDatas;
document.addEventListener('DOMContentLoaded', () => {
    var areaFile = '/data/SEC_' + ReienCode + '_' + AreaCode + '_' + SectionCode + '.json';
    fetch(areaFile)
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response error:' + response.statusText);
            }
            return response.json();
            //return response.arrayBuffer();
        })
        .then(data => {
            // 座標データ読込成功
            coordDatas = data;

            // 初期表示
            initMap();
        })
        .catch(error => {
            //console.error('There was a problem with the fetch operation:', error);
        });
});


// マップ初期表示
var corrRate = 1.0;
function initMap() {
    map = L.map('map-container', {
        crs: L.CRS.Simple,
        zoomControl: true,
        maxZoom: 1.2 // 最大ズーム（120％）
    });

    // (w,h)=(500,750) -> (w,h)=(400,600) *** #map-container(width,height)のwidthと同値を定義 ***
    var baseWidth = 500;
    var mapContainer = document.getElementById('map-container');
    var mapWidth = mapContainer.offsetWidth;
    var mapHeight = mapContainer.offsetHeight;
    if (mapWidth <= baseWidth) {
        corrRate = mapWidth / baseWidth;
    }

    var bounds = [[0, 0], [mapHeight, mapWidth]];
    var imagePath = '/images/SEC_' + ReienCode + '_' + AreaCode + '_' + SectionCode + '.png';
    var image = L.imageOverlay(imagePath, bounds).addTo(map);

    var center = [mapHeight / 2, mapWidth / 2];
    var zoomLevel = 0;
    map.setView(center, zoomLevel);
    map.setMaxBounds(bounds);

    map.on('resize', function () {
        map.setView(center, zoomLevel);
    });

    var polygons = [];
    var activePolygon = null;

    // 墓所情報
    CemeteryDatas.forEach(function (cemetery) {
        // 墓所矩形情報
        const cemeteryCoords = coordDatas.find(data => data["CemeteryCode"] == cemetery.cemeteryCode);
        createPolygon(cemeteryCoords);
    });

    // ↓↓↓ 画像作成用処理（通常コメント化）↓↓↓
    //coordDatas.forEach(function (coordData) {
    //    createPolygon(coordData);
    //});
    // ↑↑↑ 画像作成用処理（通常コメント化）↑↑↑

    // 矩形を描画
    function createPolygon(cemetery) {
        const coordinates = cemetery.Coordinates[0].map(coord => [(coord.y * corrRate), (coord.x * corrRate)]);
        var polygon = L.polygon(coordinates, {
            color: 'black',
            fillColor: 'yellow',
            fillOpacity: 0.5,
            weight: 1,
            polygonId: 'table-container-' + cemetery.CemeteryCode
        }).addTo(map);
        polygon.on('click', onPolygonClick);
        polygons.push(polygon);
    }

    // 矩形のクリックイベント
    function onPolygonClick(e) {
        var clickedLatLng = e.latlng;
        var isInsidePolygon = e.target.getBounds().contains(clickedLatLng);

        if (activePolygon) {
            activePolygon.setStyle({ fillColor: 'yellow' });
        }

        activePolygon = e.target;
        activePolygon.setStyle({ fillColor: '#ff0000' });

        var polygonId = e.target.options.polygonId;
        showTable(polygonId);
    }

    // 墓所情報の表示を描画
    function showTable(id) {
        document.querySelectorAll('div[id^="table-container"]').forEach(div => div.style.display = 'none');
        var table = document.getElementById(id);
        if (table) {
            table.style.display = 'block';
            scrollToTable(id);
        }
    }

    // スクロール設定
    function scrollToTable(tableId) {
        var table = document.getElementById(tableId);
        if (table) {
            var tablePosition = table.getBoundingClientRect().top + window.pageYOffset;
            window.scrollTo({ top: tablePosition, behavior: 'smooth' });
        }
    }
}

//获取text值，并推入数组中，主要用于搜索
var getTextValue = function (query, txtId, key) {
	var value = $('#' + txtId).val();
	if (value) {
		query.push(key + '=' + encodeURIComponent(value));
	}
};
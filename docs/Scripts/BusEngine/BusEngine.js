/* Chrome error off/block cookie "'Window': Access is denied for this document." */
try {
	window.localStorage.length;
	window.localStorage.status = true;
	window.sessionStorage.length;
	window.sessionStorage.status = true;
} catch (e) {
	delete window.localStorage;
	delete window.sessionStorage;
	window.localStorage = {
		'status':false,
		'getItem':function(a) {},
		'setItem':function(a, b) {},
		'key':function(a) {},
		'removeItem':function(a) {},
		'clear':function() {},
		'length':0
	};
	window.sessionStorage = window.localStorage;
} finally {

}

/* window.addEventListener('load', function(e) {
	if (!window.localStorage.status && !window.sessionStorage.status && !document.cookie) {
		setTimeout(function() {
			window.alert('Каб выкарыстоўваць усе магчымасці сайта, неабходна ўключыць cookie.');
		}, 1000);
	}
}); */

if (!window.console) {
	window.console = {};
	window.console.log = window.console.assert = function(){};
	window.console.warn = window.console.assert = function(){};
}

// bus_app
var busAppPostXHR = {};
var busAppbeforeinstallprompt;
if (window.navigator.onLine == false) {
	window.alert_backup = window.alert;
	window.alert = function() {};
	window.addEventListener('online', function() {
		window.alert = window.alert_backup;
	}, false);
}

if ('CacheStorage' in window && window.navigator.onLine) {
	window.caches.has('bus-app-get-1').then(function(has) {
		if (!has) {
			var XMLHttpRequestOriginal = {
				open: XMLHttpRequest.prototype.open,
				send: XMLHttpRequest.prototype.send,
			};

			XMLHttpRequest.prototype.open = function(method, url, async, user, password) {
				XMLHttpRequest.prototype.send = function(param) {
					busAppPostXHR[url + '&' + param] = {method:method, url:url, param:param, xhr:XMLHttpRequest.prototype};
					return XMLHttpRequestOriginal.send.call(this, param);
				};
				return XMLHttpRequestOriginal.open.call(this, method, url, async, user, password);
			};
		}
	});
}

window.addEventListener('beforeinstallprompt', function(event) {
	busAppbeforeinstallprompt = event;
	event.preventDefault();
}, false);

document.addEventListener('busAppBefore', function() {
	busApp.setting = {
		"version":"1.0.13.3",
		"api":"module/bus_app",
		"lg":"1",
		"md":"1",
		"sm":"1",
		"xs":"1",
		"lang":1,
		"name":"",
		"description":"Чтобы иметь быстрый доступ к сайту, нажмите УСТАНОВИТЬ! ",
		"description_ios":"Нажмите \"Поделиться\" <i class=\"share\"></i> выберите \"На экран \"Домой\" <i class=\"home\"></i>. Иконка приложения отобразится на вашем экране и вы сможете в любую минуту открыть документацию.",
		"description_android":"Нажмите \"Меню браузера\" <i class=\"fa fa-ellipsis-v\"></i> выберите \"Добавить на домашний экран\". Иконка приложения отобразится на вашем экране и вы сможете в любую минуту открыть документацию.",
		"description_bookmarks":"Установите сайт в закладки, чтобы в любую минуту открыть документацию.",
		"description_bookmarks_key":"Чтобы добавить в закладки нажмите CTRL + D на клавиатуре.","description_notification":"Чтобы быть вкурсе всех новинок, нажмите на \"Подписаться на уведомления\".",
		"appinstalled":"Всё, ты домашний!",
		"offline":"Отсутствует интернет!",
		"offline_link":"offline.html",
		"online":"Интернет появился! Ура товарищи!",
		"delay":1,
		"closeTime":300000000000,
		"cache_status":true,
		"cache_resources":[],
		"cache_resources_exception":"",
		"cache_max_ages":604800,
		"cache_token":1,
		"notification":"Для возможности подписаться на уведомления, разблокируйте эту функцию в настройках браузера.",
		"notification_status":false,
		"notification_audio":"Audio/bus_app/notification.mp3",
		"notification_interval":3600,
		"notification_error":true,
		"push_status":false,
		"push_service":0,
		"push_public_key":"",
		"sync_status":false,
		"start_url":"/",
		"route":"common/home",
		"debug":"0",
		"debug_php":false
	};
	busApp.setting['postXHR'] = busAppPostXHR;
	busApp.setting['beforeinstallprompt'] = busAppbeforeinstallprompt;
});
/* Аўтар: "БуслікДрэў" ( https://buslikdrev.by/ ) */
/* © 2016-2023; BuslikDrev - Усе правы захаваны. */
'use strict';
'use asm';
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

// Internet Explorer fix
if (!window.console) {
	window.console = {};
	window.console.log = window.console.assert = function(){};
	window.console.warn = window.console.assert = function(){};
}

var BusEngine = {};

BusEngine.cookie = {
	'set': function(name, value, domain, path, day) {
		if (typeof name == 'undefined' || typeof name != 'string') {
			return false;
		}

		if (typeof value == 'undefined' || typeof value != 'string') {
			value = '';
		}

		if (typeof domain == 'object' && domain != null) {
			console.log(domain);
			if ('path' in domain) {
				path = domain.path;
			}
			if ('day' in domain) {
				day = domain.day;
			}
			if ('domain' in domain) {
				domain = domain.domain;
			}
		}

		if (typeof domain == 'undefined' || typeof domain != 'string') {
			domain = '.' + document.domain;
		}

		if (typeof path == 'undefined' || typeof path != 'string') {
			path = '/';
		}

		if (typeof day != 'undefined' && 'Date' in window) {
			var x = new window.Date();
			x.setUTCSeconds(3600 * 24 * Number(day));
			day = ' expires=' + x + ';';
		} else {
			day = '';
		}

		document.cookie = name + '=' + value + '; path=' + path + ';' + day + ' domain=' + domain;

		return true;
	},
	'get': function(name) {
		var c = document.cookie;

		if (!c || typeof name == 'undefined' || typeof name != 'string') {
			return c;
		}

		//console.log(new RegExp('(' + name + ')\\=(\\S[^\\;]+)'));
		//console.log(/(name)\=(\S[^\;]+)/);

		c = c.match(new RegExp('(' + name + ')\\=(\\S[^\\;]+)'));

		if (c && c[2]) {
			return c[2];
		} else {
			return '';
		}
	},
	'remove': function(name, value, domain, path) {
		if (typeof name == 'undefined' || typeof name != 'string') {
			return false;
		}

		var v;

		if (typeof value == 'undefined' || typeof value != 'string') {
			v = '';
		} else {
			v = value;
		}

		if (typeof domain == 'object' && domain != null) {
			if ('path' in domain) {
				path = domain.path;
			}
			if ('domain' in domain) {
				domain = domain.domain;
			}
		}

		if (typeof domain == 'undefined' || typeof domain != 'string') {
			domain = '.' + document.domain;
		}

		if (typeof path == 'undefined' || typeof path != 'string') {
			path = '/';
		}

		document.cookie = name + '=' + v + '; expires=01 Jan 0000 00:00:00 GMT; path=' + path + '; domain=';
		document.cookie = name + '=' + v + '; expires=01 Jan 0000 00:00:00 GMT; path=' + path + '; domain=' + domain;

		return true;
	},
	'has': function(name, value) {
		var c = document.cookie;

		if (!c || typeof name == 'undefined' || typeof name != 'string') {
			return false;
		}

		c = c.match(new RegExp('(' + name + ')\\=(\\S[^\\;]+)'));

		if (typeof value == 'undefined' || typeof value != 'string') {
			if (c && c[1] && c[1] == name) {
				return true;
			}
		} else {
			if (c && c[2] && c[2] == value) {
				return true;
			}
		}

		return false;
	},
	'test': function() {
		// добавить
		BusEngine.cookie.set('BusEngine', 'Like');
		console.log(document.cookie);

		// получить
		console.log(BusEngine.cookie.get('BusEngine'));

		// проверить
		console.log(BusEngine.cookie.has('BusEngine'));

		// удалить
		BusEngine.cookie.remove('BusEngine');
		console.log(document.cookie);

		// проверить
		console.log(BusEngine.cookie.has('BusEngine'));
	}
};

BusEngine.loadScript = function(url, callback) {
	var s, ss;
	s = document.createElement('script');
	s.type = 'text/javascript';
	s.src = url;
	if (typeof callback !== 'undefined') {
		s.onreadystatechange = callback;
		s.onload = callback;
	}
	ss = document.head;
	if (ss) {
		ss.appendChild(s);
	}
};

// language
BusEngine.language = {
	'setting': {
		langDefault: 'ru',
		lang: 'ru',
		domain: document.domain
	},
	'start': function(setting) {
		if (typeof setting !== 'undefined' && typeof setting === 'object' && !('composedPath' in setting) && !('bubbles' in setting)) {
			for (var i in setting) {
				BusEngine.language.setting[i] = setting[i];
			}
		}

		if (BusEngine.cookie.has('googtrans')) {
			BusEngine.language.setting.lang = BusEngine.cookie.get('googtrans').match(/(?!^\/)[^\/]*$/gm)[0];
		} else {
			BusEngine.cookie.set('googtrans', '/auto/' + BusEngine.language.setting.lang, '', null, 365);
			BusEngine.cookie.set('googtrans', '/auto/' + BusEngine.language.setting.lang, null, null, 365);
		}

		var codest = document.querySelector('#language select');

		if (codest) {
			codest.value = BusEngine.language.setting.lang;
		}

		var x = new google.translate.TranslateElement({
			//language: 'ru',
			pageLanguage: 'ru',
			//includedLanguages: 'be,en,ru,uk',
			//layout: google.translate.TranslateElement.InlineLayout.SIMPLE
		}, 'language');
	},
	'set': function (setting) {
		if (typeof setting == 'object' && setting != null) {
			if ('lang' in setting && typeof setting.lang == 'string') {
				BusEngine.language.setting.lang = setting.lang;
			}

			BusEngine.cookie.set('googtrans', '/auto/' + BusEngine.language.setting.lang, '', null, 365);
			BusEngine.cookie.set('googtrans', '/auto/' + BusEngine.language.setting.lang, null, null, 365);

			window.location.reload();
		}
	}
};

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
		"cache_resources_exception":"https://translate.google.com/,https://www.gstatic.com/",
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

window.addEventListener('DOMContentLoaded', function() {
	BusEngine.loadScript('https://translate.google.com/translate_a/element.js?cb=BusEngine.language.start');
});
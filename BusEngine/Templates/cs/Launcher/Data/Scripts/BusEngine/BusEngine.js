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

// Fix Internet Explorer
if (!window.console) {
	window.console = {};
	window.console.log = window.console.assert = function(){};
	window.console.warn = window.console.assert = function(){};
}

// contextmenu off Internet Explorer 8+
/* document.oncontextmenu = function () {
	return false;
}; */

if (!('BusEngine' in window)) {
	window.BusEngine = {};
}
if (!('postMessage' in window.BusEngine)) {
	window.BusEngine.postMessage = function(m) {};
}

window.console.logs = window.console.log;
BusEngine.log = window.console.log = function(...args) {
	var l = new Error().stack.split('\n');
	if ('length' in l && l.length > 0) {
		l = l[l.length-1].match(/(?<=\().*?(?=\))/);
		if (l) {
			args.push(l[0]);
		}
	}
	window.console.logs.apply(this, args);
};

if (!('engine' in window.BusEngine)) {
	BusEngine.engine = {
		'settingEngine': {},
		'SettingProject': {},
	};
}

if (!('localization' in window.BusEngine)) {
	BusEngine.localization = {};
}
BusEngine.localization.initialize = function() {
	var i4, i3, l3, langs3, i2, l2, langs2, i, l, langs = document.querySelectorAll('[data-localization]');
	l = langs.length;

	langs = document.getElementsByTagName("*");
	l = langs.length;

	for (i = 0; i < l; ++i) {
		langs2 = langs[i].childNodes;
		l2 = langs2.length;

		for (i2 = 0; i2 < l2; ++i2) {
			if (langs2[i2].nodeType == Node.TEXT_NODE) {
				for (i4 in BusEngine.localization.getLanguages) {
					langs2[i2].data = langs2[i2].data.replace(new RegExp('' + String(i4).replace(/([\\\-[\]{}()*+?.,^$|])/g, '\\$1') + '', 'gim'), BusEngine.localization.getLanguages[i4]);
				}
			} else if (langs2[i2].nodeType == Node.ELEMENT_NODE) {
				for (i4 in BusEngine.localization.getLanguages) {
					if ('attributes' in langs2[i2]) {
						langs3 = langs2[i2].attributes;
						l3 = langs3.length;
	
						for (i3 = 0; i3 < l3; ++i3) {
							if (['type', 'src', 'href', 'class'].indexOf(langs3[i3].nodeName) == -1) {
								langs3[i3].value = langs3[i3].value.replace(new RegExp('' + String(i4).replace(/([\\\-[\]{}()*+?.,^$|])/g, '\\$1') + '$', 'i'), BusEngine.localization.getLanguages[i4]);
							}
						}
					}
				}
			}
		}
	}
};
if (!('getLanguages' in window.BusEngine.localization)) {
	BusEngine.localization.getLanguages = {};
}
BusEngine.localization.getLanguage = function(key) {
	if (Object.hasOwn(BusEngine.localization.getLanguages, key)) {
		return BusEngine.localization.getLanguages[key];
	} else {
		return key;
	}
};
BusEngine.localization.setLanguage = function(key, value) {
	BusEngine.localization.getLanguages[key] = value;
};

// https://developer.mozilla.org/ru/docs/Web/API/HTMLMediaElement
BusEngine.polyfillTagSource = function(ex) {
	if (typeof ex == 'undefined') {
		ex = [];
	}
	var i, l, v = document.querySelectorAll('video source[media]:not([data-error])');
	l = v.length;

	for (i = 0; i < l; ++i) {
		if (window.matchMedia(v[i].media).matches) {
			if (v[i].getAttribute('data-src') && ex.indexOf(v[i].getAttribute('data-src')) == -1) {
				v[i].setAttribute('src', v[i].getAttribute('data-src'));
				v[i].removeAttribute('data-src');
				v[i].parentNode.addEventListener('error', function(e) {
					e.target.setAttribute('data-error', e.target.src);
					ex.push(e.target.src);
					BusEngine.polyfillTagSource(ex);
				});
				v[i].parentNode.src = v[i].getAttribute('src');
				break;
			}
		} else {
			if (v[i].getAttribute('src')) {
				v[i].setAttribute('data-src', v[i].getAttribute('src'));
				v[i].removeAttribute('src');
			}
		}
	}
};

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

BusEngine.tools = {};
BusEngine.tools.ajax = function(m) {};
BusEngine.tools.json = {};
BusEngine.tools.json.encode = window.JSON.stringify;
BusEngine.tools.json.decode = window.JSON.parse;

// делаем код под стиль c#
BusEngine.PostMessage = BusEngine.postMessage;
BusEngine.Log = BusEngine.log;
BusEngine.Engine = BusEngine.engine;
BusEngine.Engine.SettingEngine = BusEngine.engine.settingEngine;
BusEngine.Engine.SettingProject = BusEngine.engine.settingProject;
BusEngine.Localization = BusEngine.localization;
BusEngine.Localization.Initialize = BusEngine.localization.initialize;
BusEngine.Localization.GetLanguages = BusEngine.localization.getLanguages;
BusEngine.Localization.GetLanguage = BusEngine.localization.getLanguage;
BusEngine.Localization.SetLanguage = BusEngine.localization.setLanguage;
BusEngine.PolyfillTagSource = BusEngine.polyfillTagSource;
BusEngine.Cookie = BusEngine.cookie;
BusEngine.LoadScript = BusEngine.loadScript;
BusEngine.Tools = BusEngine.tools;
BusEngine.Tools.Ajax = BusEngine.tools.ajax;
BusEngine.Tools.Json = BusEngine.tools.json;
BusEngine.Tools.Json.Encode = BusEngine.tools.json.encode;
BusEngine.Tools.Json.Decode = BusEngine.tools.json.decode;
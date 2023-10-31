/* Аўтар: "БуслікДрэў" ( https://buslikdrev.by/ ) */
/* © 2016-2024; BuslikDrev - Усе правы захаваны. */
'use strict';

if (!('Launcher' in window)) {
	window.Launcher = {};
}

Launcher.setSetting = function(id) {
	if (!id) {
		id = 'body';
	}

	var i, l, elements;

	setting = window.localStorage.getItem(id);

	if (setting) {
		setting = BusEngine.tools.json.decode(setting);
	} else {
		setting = {};
	}

	if (window[id]) {
		elements = window[id].querySelectorAll('[name]');
		l = elements.length;

		for (i = 0; i < l; ++i) {
			setting[elements[i].name] = elements[i].value;
		}
	}

	window.localStorage.setItem(id, BusEngine.tools.json.encode(setting));

	document.dispatchEvent(new CustomEvent('setSetting', {bubbles: true, detail: setting}));
};

Launcher.getSetting = function(id) {
	if (!id) {
		id = 'body';
	}

	var i, l, elements;

	setting = window.localStorage.getItem(id);

	if (setting) {
		setting = BusEngine.tools.json.decode(setting);
	} else {
		setting = {};
	}

	if (window[id]) {
		if (Object.keys(setting)) {
			for (i in setting) {
				elements = window[id].querySelector('[name="' + i + '"]');

				if (elements) {
					elements.value = setting[i];
				}
			}
		} else {
			elements = window[id].querySelectorAll('[name]');
			l = elements.length;

			for (i = 0; i < l; ++i) {
				setting[elements[i].name] = elements[i].value;
			}
		}
	}

	return setting;
};

Launcher.unfoldable = function(id) {
	var i, l, element, elements = document.querySelectorAll('[data-unfoldable-button]');
	l = elements.length;

	for (i = 0; i < l; ++i) {
		element = elements[i].getAttribute('data-unfoldable-button');

		if (element in window) {
			window[element]['style']['top'] = '-100vh';
		}
	}

	if (id in window && window[id]['style']['top'] == '-100vh') {
		window[id]['style']['top'] = '120px';
	}
}

Launcher.svgToggle = function(e, type) {
	if (typeof type == 'undefined') {
		type = '';
	}

	if (type == 'password') {
		var p = e.parentNode.querySelector('input');

		if (p) {
			if (p.type == 'password') {
				p.type = 'text';
			} else {
				p.type = 'password';
			}
		}
	}

	var status, i, l, elements = e.querySelectorAll('svg');
	l = elements.length;

	if (elements) {
		for (i = 0; i < l; ++i) {
			if (elements[i].style['display'] == 'none') {
				status = i;
				elements[i].style['display'] = null;
			} else {
				elements[i].style['display'] = 'none';
			}
		}
	}

	return status;
};

window.addEventListener('DOMContentLoaded', function() {
	// изменение размера окна мышкой
	var status_point, nx, ny, cursor, x, y, w, h, my_bstatus, my_status, my_x = document.querySelector('.x'), my_y = document.querySelector('.y');

	document.addEventListener('mousedown', function(e) {
		x = e.clientX;
		y = e.clientY;

		if (e.which == 1 && my_status) {
			my_bstatus = true;
			//Launcher.svgToggle(document.querySelector('#top .expand'));
			if ('content' in window) {
				window['content'].style['overflow-y'] = 'unset';
			}
		}
	});

	document.addEventListener('mouseout', function(e) {
		if (e.which != 1) {
			document.body.style['cursor'] = 'default';
			if ('content' in window) {
				window['content'].style['overflow-y'] = null;
			}
			my_bstatus = false;
			cursor = 0;
		}
	});

	document.addEventListener('mousemove', function(e) {
		if (!(window.innerWidth == window.screen.width && window.innerHeight == window.screen.height)) {
			// https://habr.com/ru/articles/509258/
			w = window.innerWidth - 10;
			h = window.innerHeight - 10;
			my_status = false;

			if (e.clientX <= 10) {
				// https://developer.mozilla.org/en-US/docs/Web/CSS/cursor
				if (!my_bstatus) {
					document.body.style['cursor'] = 'ew-resize'; // влево
					cursor = 1;
				}
				my_status = true;
			}
			if (e.clientY <= 10) {
				if (!my_bstatus) {
					document.body.style['cursor'] = 'ns-resize'; // вверх
					cursor = 3;
				}
				my_status = true;
			}
			if (e.clientX >= w) {
				if (!my_bstatus) {
					document.body.style['cursor'] = 'ew-resize'; // вправо
					cursor = 5;
				}
				my_status = true;
			}
			if (e.clientY >= h) {
				if (!my_bstatus) {
					document.body.style['cursor'] = 'ns-resize'; // вниз
					cursor = 7;
				}
				my_status = true;
			}
			if (e.clientX <= 10 && e.clientY <= 10) {
				if (!my_bstatus) {
					document.body.style['cursor'] = 'nwse-resize'; // левый-верхний
					cursor = 2;
				}
				my_status = true;
			}
			if (e.clientX >= w && e.clientY <= 10) {
				if (!my_bstatus) {
					document.body.style['cursor'] = 'nesw-resize'; // правый-верхний
					cursor = 4;
				}
				my_status = true;
			}
			if (e.clientX >= w && e.clientY >= h) {
				if (!my_bstatus) {
					document.body.style['cursor'] = 'nwse-resize'; // правый-нижний
					cursor = 6;
				}
				my_status = true;
			}
			if (e.clientX <= 10 && e.clientY >= h) {
				if (!my_bstatus) {
					document.body.style['cursor'] = 'nesw-resize'; // левый-нижний
					cursor = 8;
				}
				my_status = true;
			}

			if (e.which == 1 && my_bstatus) {
				if (cursor == 1) {
					nx = (e.clientX - x);
					ny = 0;
				} else if (cursor == 2) {
					nx = (e.clientX - x);
					ny = (e.clientY - y);
				} else if (cursor == 3) {
					nx = 0;
					ny = (e.clientY - y);
				} else if (cursor == 4) {
					nx = (e.clientX - window.innerWidth+1);
					ny = (e.clientY - y);
				} else if (cursor == 5) {
					nx = (e.clientX - window.innerWidth+1);
					ny = 0;
				} else if (cursor == 6) {
					nx = (e.clientX - window.innerWidth+1);
					ny = (e.clientY - window.innerHeight+1);
				} else if (cursor == 7) {
					nx = 0;
					ny = (e.clientY - window.innerHeight+1);
				} else if (cursor == 8) {
					nx = (e.clientX - x);
					ny = (e.clientY - window.innerHeight+1);
				}

				BusEngine.postMessage('_resize|' + nx + ' ' + ny + ' ' + cursor);
			}

			if (e.which != 1 && !my_status) {
				document.body.style['cursor'] = 'default';
				my_bstatus = false;
				cursor = 0;
			}
		}
	});

	// перемещение окна приложения
	var point = document.querySelector('#top .point');

	if (point) {
		var lx, ly, expand, p = function(e) {
			if (e.which == 1) {
				BusEngine.postMessage('__point|' + (e.clientX - Math.round(lx * window.innerWidth)) + ' ' + (e.clientY - Math.round(ly * window.innerHeight)));
				if (!(window.innerWidth == window.screen.width && window.innerHeight == window.screen.height)) {
					if (expand) {
						expand = false;
						Launcher.svgToggle(document.querySelector('#top .expand'));
					}
					if (window.screen.availTop) {

					}
				}
			}
		};

		point.addEventListener('mousedown', function(e) {
			if (window.innerWidth == window.screen.width && window.innerHeight == window.screen.height) {
				expand = true;
			}
			my_bstatus = false;
			lx = e.clientX / window.innerWidth;
			ly = e.clientY / window.innerHeight;
			e.target.style['cursor'] = 'grabbing';
			document.addEventListener('mousemove', p);
		});

		point.addEventListener('mouseup', function(e) {
			document.removeEventListener('mousemove', p);
			e.target.style['cursor'] = '';
		});

		point.addEventListener('dblclick', function(e) {
			BusEngine.postMessage('Expand');
			Launcher.svgToggle(document.querySelector('#top .expand'));
		});
	}
});
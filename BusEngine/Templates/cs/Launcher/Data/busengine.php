<?php
/* Аўтар: "БуслікДрэў" ( https://buslikdrev.by/ ) */
/* © 2016-2024; BuslikDrev - Усе правы захаваны. */

namespace BusEngine;

class Initialize {
	public function __construct($args = array()) {
		$this->Main($args);
	}

	private function Main($args = array()) {
		$data = '';

		if (!empty($args['post_message'])) {
			$t = mb_strtolower(substr($args['post_message'], 0, 8));

			if ($t == '___save|') {
				$results = json_decode(substr($args['post_message'], 8));

				if ($results && json_last_error() === JSON_ERROR_NONE) {
					$results_new = array();

					foreach ($results as $result) {
						if (array_key_exists('rus', $result) && array_key_exists('eng', $result)) {
							$f = mb_strtolower(substr($result['rus'], 0, 1));
							if (!isset($results_new[$f])) {
								$results_new[$f] = array();
							}
							$results_new[$f][] = $result;
						}
					}

					/* foreach ($results_new as $key => $result) {
						file_put_contents($_SERVER['DOCUMENT_ROOT'] . $key . '.js', json_encode($result));
					} */
				}
			}
		}

		exit($data);
	}
}
var_dump($_SERVER['DOCUMENT_ROOT']);
if ($_SERVER['REQUEST_METHOD'] != 'POST' && !empty($_POST) && is_array($_POST)) {
	new \BusEngine\Initialize($_POST);
}
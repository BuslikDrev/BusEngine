/* Аўтар: "БуслікДрэў" ( https://buslikdrev.by/ ) */
/* © 2016-2023; BuslikDrev - Усе правы захаваны. */
/* C# 6.0+ NET.Framework 4.5.2 */

public class BusEngine {
	private static void Main() {
		// https://learn.microsoft.com/ru-ru/dotnet/api/system.windows.forms.form?view=netframework-4.8
		System.Windows.Forms.Form _form = new System.Windows.Forms.Form();
		// название окна
		_form.Text = "Моя гульня!";
		// устанавливаем нашу иконку, есди она есть по пути exe, в противном случае устанавливаем системную
		if (System.IO.File.Exists(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "/My Game.ico")) {
			_form.Icon = new System.Drawing.Icon(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "/My Game.ico", 128, 128);
		} else {
			_form.Icon = new System.Drawing.Icon(System.Drawing.SystemIcons.Exclamation, 128, 128);
		}
		// устанавливаем размеры окна
		_form.Width = 1024;
		_form.Height = 768;
		// центрируем окно
		_form.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		// открываем окно на весь экран
		_form.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		// убираем линии, чтобы окно было полностью на весь экран
		_form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		// устанавливаем чёрный цвет фона окна
		_form.BackColor = System.Drawing.Color.Black;
		// выводим браузер на экран
		// https://cefsharp.github.io/api/
		_form.Controls.Add(new CefSharp.WinForms.ChromiumWebBrowser(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "/index.html"));
		// показываем форму\включаем\запускаем\стартуем показ окна
		_form.ShowDialog();
	}
}
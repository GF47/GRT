# G键快捷菜单

类似Maya的热盒操作模式, 或是Blender中的Pie, 按住G键在场景视图(Scene)中弹出可自定义的热盒, 鼠标移动至按钮, 松开G键即可触发具体方法.

## 自定义菜单项

* 在 __项目视图(Project)__ 中右键新建一个GPie配置文件

	> 在`Assets`目录下创建的名为`GPie`的配置文件会自动加载, 如果在其他位置, 则需要手动右键点击 __检视面板(Inspector)__ 的文件标题, 再点击`Use this GPie Panel`. 

* 在 __检视面板(Inspector)__ 添加菜单项

	> `Name` 菜单名称<br>
	> `Argument` 默认的字符串参数, 如果`UEvent`中的方法设置为动态参数, 则会将本参数传给方法<br>
	> `UEvent` 点击菜单执行的方法, 需要将运行方式改为`Editor And Runtime`

* 设置点击触发的方法

	GPie自身有两个默认方法:
	* `GPiePanel.ExecuteMenuItem` 可以执行引擎自身的菜单命令, 参数为菜单命令的层级路径, 每一层以`/`连接.
	* `GPiePanel.Example_PlayDefaultScene` 可以打开并运行指定的场景, 当参数为空时, 运行`Build Settings`中设置的第一个场景.

	也可以执行自定义的`ScriptableObject`中的方法, 根据具体项目而定.

* 最多设置二级, 再多了就没什么意义了

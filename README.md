0.0.8
修复了由于更新造成同性无法怀孕的bug

0.0.7
版本更新，适配游戏版本

0.0.6
<br>

增加了太吾蕾丝怀孕选择
0.0.5
<br>
添加了调整npc求爱成功率
添加了太吾怀孕机率
添加了npc怀孕机率
添加了太吾蕾丝边怀孕
添加了太吾嘿咻力解锁
添加了配偶嘿咻力解锁
美化了设置界面
0.0.4
<br>
修复了开档红字的bug
修复了个别情况下设置文件读取错误的问题

0.0.3
<br>
修复了带着小孩结婚时，由于过继机制，导致小孩父母过多的问题
修复了共结连理选项在面对已结婚对象也会显示的问题

0.0.2
<br>
增加了限制后代性别与求爱事件保底机率
0.0.1
<br>
先行测试版
由于我出谷的档坏了，暂时没有经过充分的测试，但简单测试下可以用且过月没问题



注意：由于暂时没开放事件编辑器，我是使用的其他方法patch了原版事件，
所以
<br>
勾选完要<color=#ff9900>重启游戏</color>才能起效，<color=#ff9900>重启游戏</color>，<color=#ff9900>重启游戏</color>，<color=#ff9900>重启游戏</color>>
<br>
理论上是对存档友好的，只是改变了求爱事件的判定，不会有垃圾数据，随时可以安装和卸载，不用重开档，但是不保证多配偶会对npc行为造成什么影响，这个需要后续测试
<br>
bug反馈邮箱：<color=#0042a2>xyq565861@outlook.com </color>请注明太吾绘卷mod bug反馈
<br>
<color=#ff9900>注意:</color>由于游戏更新频繁与本mod作者时间原因，此mod属于测试版本以下功能并不能保证完全没有红字。
<br>
<color=#ffffff>白色的功能：</color>此功能已经比较完善，并且理论上对存档影响较小，若开启时发生bug请第一时间通过邮箱反馈给作者，作者会尽快修复
<br>
<color=#a2a200>黄色的功能：</color>此功能已经较为完善，但理论上对存档可能造成无法立即反映出来的长远影响，或会明显影响游戏性能，请以了解这一条件的前提下自行选择开启，若开启时出现bug，请自行测试稳定复现条件后，通过邮箱反馈给作者并附上存档与复现条件说明，作者会择时修复
<br>
<color=#ff0000>红色的功能：</color>此功能为暂未经过足够测试的新功能，或功能本身存在比较大的坏档风险，但因需求人数较多所以放出，请以了解这一条件的前提下自行选择开启，若开启时出现bug，请自行测试稳定复现条件后，通过邮箱反馈给作者并附上存档与复现条件说明，作者会可能会修复
<br>
功能说明：
<br><color=#ffffff>解除求婚配偶数限制</color> 仅影响太吾，解除配偶数量限制,可以一夫多妻和一妻多夫，(<color=#ff9900>注意</color>，这会禁用原版在结婚时，若对方有小孩，将过继小孩的亲子关系的设定，仅修改了对话触发的求爱，包括被动求爱，相亲大会，迷香阵等的奇遇求爱事件未修改)
<br><color=#ffffff>解除求婚门派限制</color> 仅影响太吾，忽视求婚对象的部分身分和门派限制(<color=#ff9900>注意</color>，这会禁用远走高飞系列事件就是少林金刚五毒玄女派的私奔事件，因为会解除npc的身份等级限制，而远走高飞会让npc脱离门派，有功能的npc脱离门派后可能会发生bug，想体验剧情请第一个配偶结婚体验完再开启此功能并<color=#ff9900>重启游戏</color>)
<br><color=#ffffff>太吾求爱年龄下限</color> 太吾和目标均高于此年龄便可以求爱，游戏默认值为16岁
<br><color=#ffffff>太吾嘿咻力解锁</color> 解除年龄对太吾生育能力的影响，生育力影响春宵一刻机率与怀孕能力
<br><color=#ffffff>配偶嘿咻力解锁</color> 解除年龄对太吾配偶与情人的生育能力的影响，生育力影响春宵一刻机率与怀孕能力
<br><color=#ffffff>负责的太吾父母</color> 原版机制下，非正常春宵一刻怀孕,或新生儿父母仅为情侣，而双方有其他正式配偶时，新生儿出生不会被赋予亲子关系，打开这个选项将让父母之一为太吾的婴儿，无视双方是否有其他配偶，都会添加亲子关系，仅开启时的婴儿出生事件有效
<br><color=#a2a200>解除其他求爱限制</color> 使太吾可以向师傅，结拜兄弟，等其他部分血缘关系者主动求爱
<br><color=#a2a200>限定太吾后代性别</color> 如字面意思，只生男或者只生女，仅影响父母之一为太吾的孩子
<br><color=#ffffff>太吾求爱保底成功率</color> 仅影响太吾发起的对话触发的倾诉爱意成功率保底，忽视性别，年龄等因素，让太吾在满足好感度前提下对任何人告白都有保底概率可以成功，原版机制与配偶数量，相貌，年龄差，魅力，性别取向均有关系，此处为保底率，成功率将会不低于此值。0代表无保底成功率，3代表保底成功率30%，8代表保底成功率80%，以此类推
<br><color=#ff0000>npc求爱成功率</color> 全局影响所有npc求爱成功率，修改此值可能会造成游戏世界人口不正常波动
<br><color=#a2a200>放止太吾被绿</color> 大幅降低太吾配偶与情人的新欢的概率（无法降到0）
<br><color=#a2a200>太吾怀孕机率</color> 仅影响太吾参与的春宵一刻事件，其中<color=#0042a2>绝对避孕</color>为完全避孕。<color=#0042a2>机率减半</color>与<color=#0042a2>机率减半</color>仅影响包含临时随机数，生育力，世界人口调整数在内的调整值，不影响女性生育冷却期，<color=#0042a2>绝对怀孕</color>可突破世界人口调整值，但仍受女性生育冷却期限制
<br><color=#ff0000>npc怀孕机率</color> 全局影响所有npc怀孕机率,仅影响包含临时随机数，生育力，世界人口调整数在内的调整值，但仍受女性生育冷却期影响
<br><color=#ff0000>太吾蕾丝边怀孕</color> 仅影响太吾参与的春宵一刻事件，解除怀孕事件双方性别限制
<br><color=#ff0000>debugMode</color> 不要开启，仅调试使用，会在你的游戏文件夹生成一堆对你没用的日志
<br>
使用方法解压到游戏目录下的mod然后在模组管理器中勾选相应内容，debug不要选，会在你的游戏文件夹生成一堆对你没用的日志

CONST ResonForRejectioh = "不好意思，我现在没空"
->main

=== main ===
你好，冒险家。我有一个重要的任务要交给你。#speaker:委托人
    *[什么任务？]
        什么任务？，我很感兴趣。#speaker:冒险家
    ->choice1
    *[你是谁？]
        你是谁？为什么找我？#speaker:冒险家
    ->choice2
    *[拒绝] 
        {ResonForRejectioh} #speaker:冒险家
    ->choice3
-> END

=== choice1 ===
你的任务是非常重要的，你必须尽快完成。我需要你去一个叫做<color=\#0000FF>阿克罗波利斯</color>的遗迹，寻找<color=\#0000FF>一件古老的神器</color>，它可以控制时间的流动。这个遗迹隐藏在<color=\#0000FF>地下深处</color>，你要小心那里的陷阱和怪物。神器的形状是一个<color=\#0000FF>金色的沙漏</color>，你一定要把它带回来。#speaker:委托人
    *[接受]
        我明白了，我接受这个任务。#speaker:冒险家
    ->choice4
    *[拒绝]
        {ResonForRejectioh} #speaker:冒险家
    ->choice3

=== choice2 ===
我是一个考古学家，专门研究古代文明。我找你是因为你有着丰富的探险经验。#speaker:委托人
    * [询问委托内容]
        哦，原来是这样。那么你要我做什么？ #speaker:冒险家
    ->choice1
->END

=== choice3 ===
好吧，那可真是太可惜了。要是你回心转意了，就回来找我，我通常都会在这附近。#speaker:委托人
->END

=== choice4 ===
噢，你可真是个勇敢的冒险家，我会在这里等你的好消息的。#speaker:委托人
->END
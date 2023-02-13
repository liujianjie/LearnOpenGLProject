#include <glad/glad.h>
#include <GLFW/glfw3.h>
#include <iostream>

// 按键事件
void processInput(GLFWwindow* window)
{
    if (glfwGetKey(window, GLFW_KEY_ESCAPE) == GLFW_PRESS)
        glfwSetWindowShouldClose(window, true);
}

void framebuffer_size_callback(GLFWwindow* window, int width, int height)
{
    glViewport(0, 0, width, height);
}
int main()
{
    glfwInit();
    glfwWindowHint(GLFW_CONTEXT_VERSION_MAJOR, 3);// 主版本号(Major) 版本
    glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, 3);// 次版本号(Minor)
    glfwWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_CORE_PROFILE);// 核心模式(Core-profile)
    //glfwWindowHint(GLFW_OPENGL_FORWARD_COMPAT, GL_TRUE);

    // 这是窗口
    GLFWwindow* window = glfwCreateWindow(800, 600, "LearnOpenGL", NULL, NULL);
    if (window == NULL)
    {
        std::cout << "Failed to create GLFW window" << std::endl;
        glfwTerminate();
        return -1;
    }
    glfwMakeContextCurrent(window);// 将我们窗口的上下文设置为当前线程的主上下文

    // glad加载OpenGL函数，glfw提供OpenGL函数指针地址
    if (!gladLoadGLLoader((GLADloadproc)glfwGetProcAddress))
    {
        std::cout << "Failed to initialize GLAD" << std::endl;
        return -1;
    }
    // OpenGL渲染窗口的尺寸大小，即视口(Viewport)
    glViewport(0, 0, 800, 600);// 0 0 左下角,800 600 宽高
    // 设置窗口改变视口也改变
    glfwSetFramebufferSizeCallback(window, framebuffer_size_callback);
    // 循环渲染
    while (!glfwWindowShouldClose(window))// 检查一次GLFW是否被要求退出
    {
        glClearColor(0.2f, 0.3f, 0.3f, 1.0f);
        glClear(GL_COLOR_BUFFER_BIT);
        glfwSwapBuffers(window);// 函数会交换颜色缓冲：单缓冲可能会造成屏幕闪烁
        glfwPollEvents();// 函数检查有没有触发什么事件
        processInput(window);
    }
    // 释放/删除之前的分配的所有资源
    glfwTerminate();
    return 0;
}